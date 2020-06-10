using Leopotam.Ecs;

namespace Game.Portals {
    public struct PortalComponent {
        public EcsEntity otherPortalEntity;
        public float estimateReloadTime;
    }
}