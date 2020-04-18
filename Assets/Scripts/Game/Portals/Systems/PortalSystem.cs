using Game.Moving;
using Game.Teleports;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Portals {
    public class PortalSystem : IEcsRunSystem {
        private readonly WorldService _worldService = null;
        private readonly GameDefinitions _gameDefinitions = null;
        private readonly EcsFilter<PortalComponent> _portals = null;
        private readonly EcsFilter<NewPositionEvent, MoveComponent> _moveObjects = null;

        public void Run() {
            foreach (int i in _moveObjects) {
                Vector2Int newPosition = _moveObjects.Get1(i).NewPosition;
                EcsEntity movableEntity = _moveObjects.GetEntity(i);

                foreach (EcsEntity entity in _worldService.WorldField[newPosition.x][newPosition.y]) {
                    if (!entity.Has<PortalComponent>()) continue;

                    ref var portal = ref entity.Set<PortalComponent>();
                    if (portal.EstimateReloadTime > 0) continue;

                    EcsEntity otherPortalEntity = portal.OtherPortalEntity;
                    Vector2Int otherPortalPosition = otherPortalEntity.Set<PositionComponent>().Position;
                    movableEntity.Set<TeleportedEvent>().NewPosition = otherPortalPosition;

                    portal.EstimateReloadTime = _gameDefinitions.portalDefinition.portalReloadTime;
                    otherPortalEntity.Set<PortalComponent>().EstimateReloadTime = _gameDefinitions.portalDefinition.portalReloadTime;
                }
            }

            foreach (int i in _portals) {
                PortalComponent portalComponent = _portals.Get1(i);
                if (portalComponent.EstimateReloadTime > 0) {
                    portalComponent.EstimateReloadTime -= Time.deltaTime;
                }
            }
        }
    }
}