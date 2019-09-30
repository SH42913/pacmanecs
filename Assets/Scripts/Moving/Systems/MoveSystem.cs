using System;
using Leopotam.Ecs;
using UnityEngine;
using Walls;
using World;

namespace Moving.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        private const float Epsilon = 0.1f;

        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<PositionComponent, MoveComponent, WorldObjectComponent> _moveEntities = null;

        public void Run()
        {
            WorldComponent world = _world.Get1[0];
            foreach (int i in _moveEntities)
            {
                PositionComponent positionComponent = _moveEntities.Get1[i];
                MoveComponent moveComponent = _moveEntities.Get2[i];
                Transform transform = _moveEntities.Get3[i].Transform;
                EcsEntity movingEntity = _moveEntities.Entities[i];

                Vector3 curPosition = transform.position;
                float height = curPosition.y;
                Vector3 desiredPosition = moveComponent.DesiredPosition.ToVector3(height);
                Vector3 estimatedVector = desiredPosition - curPosition;
                if (estimatedVector.magnitude > Epsilon)
                {
                    transform.position = Vector3.Lerp(
                        transform.position, desiredPosition,
                        moveComponent.Speed / estimatedVector.magnitude * Time.deltaTime);
                    continue;
                }

                Vector2Int oldPosition = positionComponent.Position;
                Vector2Int newPosition = moveComponent.DesiredPosition;
                if (!oldPosition.Equals(newPosition))
                {
                    movingEntity.Set<NewPositionComponent>().NewPosition = newPosition;
                }

                Vector2Int newDesiredPosition;
                Vector3 newDirection;
                switch (moveComponent.Heading)
                {
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

                transform.rotation = Quaternion.Euler(newDirection);

                bool stuckToWall = false;
                foreach (EcsEntity entity in world.WorldField[newDesiredPosition.x][newDesiredPosition.y])
                {
                    if (!entity.IsAlive()) continue;
                    if (entity.Get<WallComponent>() == null) continue;

                    stuckToWall = true;
                }

                if (stuckToWall)
                {
                    movingEntity.Set<StoppedComponent>();
                }
                else
                {
                    moveComponent.DesiredPosition = newDesiredPosition;
                    movingEntity.Unset<StoppedComponent>();
                }
            }
        }
    }
}