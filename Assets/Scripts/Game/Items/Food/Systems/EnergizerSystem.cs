using Game.Ghosts;
using Leopotam.Ecs;

namespace Game.Items.Food {
    public sealed class EnergizerSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<EnergizerComponent, TakenItemEvent> takenEnergizers = null;

        public void Run() {
            if (!takenEnergizers.IsEmpty()) {
                ecsWorld.NewEntity().Get<GhostStartFearStateEvent>();
            }
        }
    }
}