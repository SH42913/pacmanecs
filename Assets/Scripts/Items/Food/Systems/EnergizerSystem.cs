using Ghosts;
using Leopotam.Ecs;

namespace Items.Food.Systems
{
    [EcsInject]
    public class EnergizerSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<EnergizerComponent, TakenItemComponent> _takenEnergizers = null;

        public void Run()
        {
            if (_takenEnergizers.EntitiesCount > 0)
            {
                _ecsWorld.CreateEntityWith<EnableGhostFearStateEvent>();
            }
        }
    }
}