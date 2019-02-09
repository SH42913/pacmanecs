using Leopotam.Ecs;
using Moving;
using Teleports;
using UnityEngine;
using World;

namespace Portals.Systems
{
    [EcsInject]
    public class PortalSystem : IEcsRunSystem
    {
        private const float PORTAL_RELOAD_TIME = 1f;

        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<PortalComponent> _portals = null;
        private readonly EcsFilter<NewPositionComponent, MoveComponent> _moveableObjects = null;

        public void Run()
        {
            WorldComponent world = _world.Components1[0];

            foreach (int i in _moveableObjects)
            {
                Vector2Int newPosition = _moveableObjects.Components1[i].NewPosition;
                int movableEntity = _moveableObjects.Entities[i];

                foreach (int entity in world.WorldField[newPosition.x][newPosition.y])
                {
                    var portal = _ecsWorld.GetComponent<PortalComponent>(entity);
                    if (portal == null || portal.EstimateReloadTime > 0) continue;

                    int otherPortalEntity = portal.OtherPortalEntity;
                    var otherPortal = _ecsWorld.GetComponent<PortalComponent>(otherPortalEntity);

                    Vector2Int otherPortalPosition = _ecsWorld
                        .GetComponent<PositionComponent>(otherPortalEntity)
                        .Position;
                    _ecsWorld.AddComponent<TeleportingComponent>(movableEntity).NewPosition = otherPortalPosition;

                    portal.EstimateReloadTime = PORTAL_RELOAD_TIME;
                    otherPortal.EstimateReloadTime = PORTAL_RELOAD_TIME;
                }
            }

            foreach (int i in _portals)
            {
                PortalComponent portalComponent = _portals.Components1[i];
                if (portalComponent.EstimateReloadTime > 0)
                {
                    portalComponent.EstimateReloadTime -= Time.deltaTime;
                }
            }
        }
    }
}