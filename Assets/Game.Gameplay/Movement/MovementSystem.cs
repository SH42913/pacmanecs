using Game.Gameplay.Walls;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Movement {
    public sealed class MovementSystem : IEcsRunSystem {
        private const float epsilon = 0.1f;

        private readonly WorldService worldService = null;
        private readonly EcsFilter<PositionComponent, MovementComponent, WorldObjectComponent> moveEntities = null;

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

        private static Vector2Int UpdatePosition(in EcsEntity movingEntity, ref MovementComponent movement, Transform transform, in Vector2Int oldPosition) {
            var newPosition = movement.desiredPosition;
            if (!oldPosition.Equals(newPosition)) {
                movingEntity.Get<WorldObjectNewPositionRequest>().newPosition = newPosition;
            }

            transform.rotation = movement.heading.GetRotation();
            return movement.heading.GetPosition(newPosition);
        }

        private void CheckStuckToWall(in EcsEntity movingEntity, ref MovementComponent movement, in Vector2Int newDesiredPosition) {
            var stuckToWall = false;
            foreach (var entity in worldService.GetEntitiesOn(newDesiredPosition)) {
                if (entity.IsAlive() && entity.Has<WallMarker>()) {
                    stuckToWall = true;
                }
            }

            if (stuckToWall) {
                movingEntity.Get<MovementStoppedMarker>();
            } else {
                movement.desiredPosition = newDesiredPosition;
                movingEntity.Del<MovementStoppedMarker>();
            }
        }
    }
}