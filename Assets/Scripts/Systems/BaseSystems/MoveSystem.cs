using System;
using Components.BaseComponents;
using Components.StaticComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.BaseSystems
{
    [EcsInject]
    public class MoveSystem : IEcsRunSystem
    {
        private EcsFilter<PositionComponent, MoveComponent> _moveComponents = null;
        private EcsFilter<PositionComponent, WallComponent> _walls = null;
        
        public void Run()
        {
            for (int i = 0; i < _moveComponents.EntitiesCount; i++)
            {
                var moveComponent = _moveComponents.Components2[i];
                Vector3 estimatedVector = moveComponent.DesiredPosition - moveComponent.Transform.position;

                if (estimatedVector.magnitude > 0.1f)
                {
                    moveComponent.Transform.position = Vector3.Lerp(
                        moveComponent.Transform.position,
                        moveComponent.DesiredPosition,
                        moveComponent.Speed/estimatedVector.magnitude * Time.deltaTime);
                    continue;
                }
                
                _moveComponents.Components1[i].Position = moveComponent.Transform.position.ToVector2Int();
                Vector2Int newPosition;
                Vector3 newDirection;
                var position = _moveComponents.Components1[i].Position;
                switch (moveComponent.Heading)
                {
                    case Directions.UP:
                        newPosition = new Vector2Int(position.x, position.y + 1);
                        newDirection = new Vector3(0, 0, 0);
                        break;
                    case Directions.RIGHT:
                        newPosition = new Vector2Int(position.x + 1, position.y);
                        newDirection = new Vector3(0, 90, 0);
                        break;
                    case Directions.DOWN:
                        newPosition = new Vector2Int(position.x, position.y - 1);
                        newDirection = new Vector3(0, 180, 0);
                        break;
                    case Directions.LEFT:
                        newPosition = new Vector2Int(position.x - 1, position.y);
                        newDirection = new Vector3(0, -90, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                moveComponent.Transform.rotation = Quaternion.Euler(newDirection);

                if(_walls.GetFirstComponent(x => x.Position == newPosition) != null) continue;
                moveComponent.DesiredPosition = newPosition.ToVector3(moveComponent.Transform.position.y);
            }
        }
    }
}