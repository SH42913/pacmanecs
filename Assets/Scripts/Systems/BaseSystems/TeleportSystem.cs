using Components.BaseComponents;
using LeopotamGroup.Ecs;

namespace Systems.BaseSystems
{
    [EcsInject]
    public class TeleportSystem : IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<TeleportComponent> Components { get; set; }
        private EcsFilter<PositionComponent, MoveComponent> Moveables { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < Components.EntitiesCount; i++)
            {
                var teleportComponent = Components.Components1[i];

                for (int j = 0; j < Moveables.EntitiesCount; j++)
                {
                    var moveComponent = Moveables.Components2[j];
                    if(!moveComponent.Equals(teleportComponent.MoveComponent)) continue;

                    var positionComponent = Moveables.Components1[j];
                    positionComponent.Position = teleportComponent.TargetPosition.ToVector2Int();
                    moveComponent.DesiredPosition = teleportComponent.TargetPosition;
                    moveComponent.Transform.position = teleportComponent.TargetPosition;
                }
                
                EcsWorld.RemoveEntity(Components.Entities[i]);
            }
        }
    }
}