using Leopotam.Ecs;

namespace Game.Gameplay.Portals {
    public struct PortalComponent {
        public EcsEntity otherPortalEntity;
        public float estimateReloadTime;
    }
}