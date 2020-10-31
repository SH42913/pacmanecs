using Game.Gameplay.Ghosts;
using Leopotam.Ecs;

namespace Game.Gameplay.Items.Food {
    public sealed class EnergizerSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<EnergizerMarker, ItemTakenEvent> takenEnergizers = null;

        public void Run() {
            if (!takenEnergizers.IsEmpty()) {
                ecsWorld.NewEntity().Get<GhostFearStateRequest>();
            }
        }
    }
}