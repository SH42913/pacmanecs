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
        private EcsFilter<PositionComponent, MoveComponent> Components { get; set; }
        private EcsFilter<PositionComponent, WallComponent> Walls { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < Components.EntitiesCount; i++)
            {
                var moveComponent = Components.Components2[i];
                Vector3 estimatedVector = moveComponent.DesiredPosition - moveComponent.Transform.position;

                if (estimatedVector.magnitude > 0.1f)
                {
                    moveComponent.Transform.position = Vector3.Lerp(
                        moveComponent.Transform.position,
                        moveComponent.DesiredPosition,
                        moveComponent.Speed/estimatedVector.magnitude * Time.deltaTime);
                    continue;
                }
                
                Components.Components1[i].Position = moveComponent.Transform.position.ToVector2Int();
                Vector2Int newPosition;
                Vector3 newDirection;
                var position = Components.Components1[i].Position;
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

                if(Walls.GetSecondComponent(x => x.Position == newPosition) != null) continue;
                moveComponent.DesiredPosition = newPosition.ToVector3(moveComponent.Transform.position.y);
            }
        }
    }
}