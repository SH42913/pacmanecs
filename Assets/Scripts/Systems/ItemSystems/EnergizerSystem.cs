using Components.BaseComponents;
using Components.ItemComponents;
using Components.PlayerComponents;
using Leopotam.Ecs;

namespace Systems.ItemSystems
{
    [EcsInject]
    public class EnergizerSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<EnergizerComponent> _components = null;
        
        public void Run()
        {
            for (int i = 0; i < _components.EntitiesCount; i++)
            {
                int playerEntity = _components.Components1[i].PlayerEntity;
                var player = _ecsWorld.GetComponent<PlayerComponent>(playerEntity);
                var moveComponent = _ecsWorld.GetComponent<MoveComponent>(playerEntity);

                player.Scores += _components.Components1[i].Scores;
                moveComponent.Speed -= _components.Components1[i].SpeedPenalty;
                
                //ToDo Ghost fear mode enable
                
                _ecsWorld.RemoveEntity(_components.Entities[i]);
                _ecsWorld.CreateEntityWith<UpdateGuiComponent>();
            }
        }
    }
}