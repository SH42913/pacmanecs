using Game.Ghosts;
using Leopotam.Ecs;

namespace Game.Items.Food
{
    public class EnergizerSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<EnergizerComponent, TakenItemComponent> _takenEnergizers = null;

        public void Run()
        {
            if (!_takenEnergizers.IsEmpty())
            {
                _ecsWorld.NewEntityWith(out EnableGhostFearStateEvent _);
            }
        }
    }
}