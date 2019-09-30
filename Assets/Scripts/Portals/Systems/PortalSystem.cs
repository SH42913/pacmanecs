using Leopotam.Ecs;
using Moving;
using Teleports;
using UnityEngine;
using World;

namespace Portals.Systems
{
    public class PortalSystem : IEcsRunSystem
    {
        private const float PortalReloadTime = 1f;

        private readonly WorldService _worldService = null;
        private readonly EcsFilter<PortalComponent> _portals = null;
        private readonly EcsFilter<NewPositionComponent, MoveComponent> _moveObjects = null;

        public void Run()
        {
            foreach (int i in _moveObjects)
            {
                Vector2Int newPosition = _moveObjects.Get1[i].NewPosition;
                EcsEntity movableEntity = _moveObjects.Entities[i];

                foreach (EcsEntity entity in _worldService.WorldField[newPosition.x][newPosition.y])
                {
                    var portal = entity.Get<PortalComponent>();
                    if (portal == null || portal.EstimateReloadTime > 0) continue;

                    EcsEntity otherPortalEntity = portal.OtherPortalEntity;
                    var otherPortal = otherPortalEntity.Get<PortalComponent>();

                    Vector2Int otherPortalPosition = otherPortalEntity.Get<PositionComponent>().Position;
                    movableEntity.Set<TeleportedComponent>().NewPosition = otherPortalPosition;

                    portal.EstimateReloadTime = PortalReloadTime;
                    otherPortal.EstimateReloadTime = PortalReloadTime;
                }
            }

            float dt = Time.deltaTime;
            foreach (int i in _portals)
            {
                PortalComponent portalComponent = _portals.Get1[i];
                if (portalComponent.EstimateReloadTime > 0)
                {
                    portalComponent.EstimateReloadTime -= dt;
                }
            }
        }
    }
}