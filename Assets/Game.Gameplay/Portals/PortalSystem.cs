using Game.Gameplay.Movement;
using Game.Gameplay.Teleports;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Portals {
    public sealed class PortalSystem : IEcsRunSystem {
        private readonly WorldService worldService = null;
        private readonly GameDefinitions gameDefinitions = null;

        private readonly EcsFilter<PortalComponent> portals = null;
        private readonly EcsFilter<WorldObjectNewPositionRequest, MovementComponent> moveObjects = null;

        public void Run() {
            foreach (var i in moveObjects) {
                var newPosition = moveObjects.Get1(i).newPosition;
                var movableEntity = moveObjects.GetEntity(i);
                CheckPortalInPosition(movableEntity, newPosition);
            }

            foreach (var i in portals) {
                ref var portalComponent = ref portals.Get1(i);
                if (portalComponent.estimateReloadTime > 0) {
                    portalComponent.estimateReloadTime -= Time.deltaTime;
                }
            }
        }

        private void CheckPortalInPosition(in EcsEntity movableEntity, in Vector2Int newPosition) {
            foreach (var entity in worldService.GetEntitiesOn(newPosition)) {
                if (!entity.Has<PortalComponent>()) continue;

                ref var portal = ref entity.Get<PortalComponent>();
                if (portal.estimateReloadTime > 0) continue;

                var otherPortalEntity = portal.otherPortalEntity;
                var otherPortalPosition = otherPortalEntity.Get<PositionComponent>().position;
                movableEntity.Get<TeleportToPositionRequest>().newPosition = otherPortalPosition;

                portal.estimateReloadTime = gameDefinitions.portalDefinition.portalReloadTime;
                otherPortalEntity.Get<PortalComponent>().estimateReloadTime = gameDefinitions.portalDefinition.portalReloadTime;
            }
        }
    }
}