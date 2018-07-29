using Components.BaseComponents;
using Leopotam.Ecs;

namespace Systems.BaseSystems
{
    [EcsInject]
    public class TeleportSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<TeleportComponent> _components = null;
        
        public void Run()
        {
            for (int teleportIndex = 0; teleportIndex < _components.EntitiesCount; teleportIndex++)
            {
                var teleportComponent = _components.Components1[teleportIndex];
                var moveEntity = teleportComponent.MoveEntity;

                var moveComponent = _ecsWorld.GetComponent<MoveComponent>(moveEntity);
                var positionComponent = _ecsWorld.GetComponent<PositionComponent>(moveEntity);
                if (moveComponent != null && positionComponent != null)
                {
                    positionComponent.Position = teleportComponent.TargetPosition.ToVector2Int();
                    moveComponent.DesiredPosition = teleportComponent.TargetPosition;
                    moveComponent.Transform.position = teleportComponent.TargetPosition;
                }
                
                _ecsWorld.RemoveEntity(_components.Entities[teleportIndex]);
            }
        }
    }
}