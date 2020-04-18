using Leopotam.Ecs;

namespace Game.Portals {
    public struct PortalComponent {
        public EcsEntity OtherPortalEntity;
        public float EstimateReloadTime;
    }
}