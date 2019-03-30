using System;
using Leopotam.Ecs;
using UnityEngine;
using Walls;
using World;

namespace Moving.Systems
{
    [EcsInject]
    public class MoveSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<PositionComponent, MoveComponent, WorldObjectComponent> _moveableEntities = null;

        public void Run()
        {
            WorldComponent world = _world.Components1[0];
            foreach (int i in _moveableEntities)
            {
                PositionComponent positionComponent = _moveableEntities.Components1[i];
                MoveComponent moveComponent = _moveableEntities.Components2[i];
                Transform transform = _moveableEntities.Components3[i].Transform;
                int movingEntity = _moveableEntities.Entities[i];

                float height = transform.position.y;
                Vector3 desiredPosition = moveComponent.DesiredPosition.ToVector3(height);
                Vector3 estimatedVector = desiredPosition - transform.position;
                if (estimatedVector.magnitude > 0.1f)
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
                    _ecsWorld.AddComponent<NewPositionComponent>(movingEntity).NewPosition = newPosition;
                }

                Vector2Int newDesiredPosition;
                Vector3 newDirection;
                switch (moveComponent.Heading)
                {
                    case Directions.UP:
                        newDesiredPosition = new Vector2Int(newPosition.x, newPosition.y + 1);
                        newDirection = new Vector3(0, 0, 0);
                        break;
                    case Directions.RIGHT:
                        newDesiredPosition = new Vector2Int(newPosition.x + 1, newPosition.y);
                        newDirection = new Vector3(0, 90, 0);
                        break;
                    case Directions.DOWN:
                        newDesiredPosition = new Vector2Int(newPosition.x, newPosition.y - 1);
                        newDirection = new Vector3(0, 180, 0);
                        break;
                    case Directions.LEFT:
                        newDesiredPosition = new Vector2Int(newPosition.x - 1, newPosition.y);
                        newDirection = new Vector3(0, -90, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                transform.rotation = Quaternion.Euler(newDirection);

                bool stuckToWall = false;
                foreach (int entity in world.WorldField[newDesiredPosition.x][newDesiredPosition.y])
                {
                    if (!_ecsWorld.IsEntityExists(entity)) continue;
                    if (_ecsWorld.GetComponent<WallComponent>(entity) == null) continue;

                    stuckToWall = true;
                }

                if (stuckToWall)
                {
                    _ecsWorld.EnsureComponent<StoppedComponent>(movingEntity, out _);
                }
                else
                {
                    moveComponent.DesiredPosition = newDesiredPosition;
                    _ecsWorld.RemoveComponent<StoppedComponent>(movingEntity, true);
                }
            }
        }
    }
}