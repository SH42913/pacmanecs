using Components.BaseComponents;
using Leopotam.Ecs;

namespace Systems.BaseSystems
{
    [EcsInject]
    public class TeleportSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<TeleportComponent> _components = null;
        private EcsFilter<PositionComponent, MoveComponent> _moveables = null;
        
        public void Run()
        {
            for (int i = 0; i < _components.EntitiesCount; i++)
            {
                var teleportComponent = _components.Components1[i];

                for (int j = 0; j < _moveables.EntitiesCount; j++)
                {
                    var moveComponent = _moveables.Components2[j];
                    if(!moveComponent.Equals(teleportComponent.MoveComponent)) continue;

                    var positionComponent = _moveables.Components1[j];
                    positionComponent.Position = teleportComponent.TargetPosition.ToVector2Int();
                    moveComponent.DesiredPosition = teleportComponent.TargetPosition;
                    moveComponent.Transform.position = teleportComponent.TargetPosition;
                }
                
                _ecsWorld.RemoveEntity(_components.Entities[i]);
            }
        }
    }
}