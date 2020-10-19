using System;
using Utils;
using Game.Walls;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Moving {
    public sealed class MoveSystem : IEcsRunSystem {
        private const float epsilon = 0.1f;

        private readonly WorldService worldService = null;
        private readonly EcsFilter<PositionComponent, MoveComponent, WorldObjectComponent> moveEntities = null;

        public void Run() {
            foreach (var i in moveEntities) {
                var movingEntity = moveEntities.GetEntity(i);
                ref var positionComponent = ref moveEntities.Get1(i);
                ref var moveComponent = ref moveEntities.Get2(i);
                var transform = moveEntities.Get3(i).transform;

                var curPosition = transform.position;
                var desiredPosition = moveComponent.desiredPosition.ToVector3(curPosition.y);
                var estimatedVector = desiredPosition - curPosition;
                if (estimatedVector.magnitude > epsilon) {
                    transform.position = Vector3.Lerp(transform.position, desiredPosition,
                                                      moveComponent.speed / estimatedVector.magnitude * Time.deltaTime);
                    continue;
                }

                var newDesiredPosition = UpdatePosition(movingEntity, ref moveComponent, transform, positionComponent.position);
                CheckStuckToWall(movingEntity, ref moveComponent, newDesiredPosition);
            }
        }

        private static Vector2Int UpdatePosition(EcsEntity movingEntity, ref MoveComponent moveComponent, Transform transform, in Vector2Int oldPosition) {
            var newPosition = moveComponent.desiredPosition;
            if (!oldPosition.Equals(newPosition)) {
                movingEntity.Get<NewPositionEvent>().newPosition = newPosition;
            }

            transform.rotation = moveComponent.heading.GetRotation();
            return moveComponent.heading.GetPosition(newPosition);
        }

        private void CheckStuckToWall(EcsEntity movingEntity, ref MoveComponent moveComponent, Vector2Int newDesiredPosition) {
            var stuckToWall = false;
            foreach (var entity in worldService.worldField[newDesiredPosition.x][newDesiredPosition.y]) {
                if (entity.IsAlive() && entity.Has<WallComponent>()) {
                    stuckToWall = true;
                }
            }

            if (stuckToWall) {
                movingEntity.Get<StoppedComponent>();
            } else {
                moveComponent.desiredPosition = newDesiredPosition;
                movingEntity.Del<StoppedComponent>();
            }
        }
    }
}