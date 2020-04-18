using System;
using Utils;
using Game.Walls;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Moving {
    public class MoveSystem : IEcsRunSystem {
        private const float Epsilon = 0.1f;

        private readonly WorldService _worldService = null;
        private readonly EcsFilter<PositionComponent, MoveComponent, WorldObjectComponent> _moveEntities = null;

        public void Run() {
            foreach (int i in _moveEntities) {
                EcsEntity movingEntity = _moveEntities.GetEntity(i);
                ref PositionComponent positionComponent = ref _moveEntities.Get1(i);
                ref MoveComponent moveComponent = ref _moveEntities.Get2(i);
                Transform transform = _moveEntities.Get3(i).Transform;

                Vector3 curPosition = transform.position;
                Vector3 desiredPosition = moveComponent.DesiredPosition.ToVector3(curPosition.y);
                Vector3 estimatedVector = desiredPosition - curPosition;
                if (estimatedVector.magnitude > Epsilon) {
                    transform.position = Vector3.Lerp(
                        transform.position, desiredPosition,
                        moveComponent.Speed / estimatedVector.magnitude * Time.deltaTime);
                    continue;
                }

                Vector2Int oldPosition = positionComponent.Position;
                Vector2Int newPosition = moveComponent.DesiredPosition;
                if (!oldPosition.Equals(newPosition)) {
                    movingEntity.Set<NewPositionEvent>().NewPosition = newPosition;
                }

                GetPositions(ref moveComponent, newPosition, out var newDesiredPosition, out var newDirection);

                transform.rotation = Quaternion.Euler(newDirection);

                bool stuckToWall = false;
                foreach (EcsEntity entity in _worldService.WorldField[newDesiredPosition.x][newDesiredPosition.y]) {
                    if (!entity.IsAlive() || !entity.Has<WallComponent>()) continue;

                    stuckToWall = true;
                }

                if (stuckToWall) {
                    movingEntity.Set<StoppedComponent>();
                }
                else {
                    moveComponent.DesiredPosition = newDesiredPosition;
                    movingEntity.Unset<StoppedComponent>();
                }
            }
        }

        private static void GetPositions(ref MoveComponent moveComponent, Vector2Int newPosition, out Vector2Int newDesiredPosition, out Vector3 newDirection) {
            switch (moveComponent.Heading) {
                case Directions.Up:
                    newDesiredPosition = new Vector2Int(newPosition.x, newPosition.y + 1);
                    newDirection = new Vector3(0, 0, 0);
                    break;
                case Directions.Right:
                    newDesiredPosition = new Vector2Int(newPosition.x + 1, newPosition.y);
                    newDirection = new Vector3(0, 90, 0);
                    break;
                case Directions.Down:
                    newDesiredPosition = new Vector2Int(newPosition.x, newPosition.y - 1);
                    newDirection = new Vector3(0, 180, 0);
                    break;
                case Directions.Left:
                    newDesiredPosition = new Vector2Int(newPosition.x - 1, newPosition.y);
                    newDirection = new Vector3(0, -90, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}