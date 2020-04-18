using Game.Ghosts;
using Leopotam.Ecs;

namespace Game.Items.Food {
    public class EnergizerSystem : IEcsRunSystem {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<EnergizerComponent, TakenItemEvent> _takenEnergizers = null;

        public void Run() {
            if (!_takenEnergizers.IsEmpty()) {
                _ecsWorld.NewEntity().Set<EnableGhostFearStateEvent>();
            }
        }
    }
}