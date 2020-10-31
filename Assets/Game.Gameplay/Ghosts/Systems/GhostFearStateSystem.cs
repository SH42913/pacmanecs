using System;
using Game.Gameplay.Players;
using Game.Gameplay.Ui.ScoreTable;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Ghosts {
    public sealed class GhostFearStateSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly WorldService worldService = null;
        private readonly GameDefinitions gameDefinitions = null;

        private readonly EcsFilter<GhostComponent> ghosts = null;
        private readonly EcsFilter<GhostFearStateRequest> requests = null;
        private readonly EcsFilter<GhostComponent, GhostInFearStateComponent> fearStateGhosts = null;

        public void Run() {
            var ghostDefinition = gameDefinitions.ghostDefinition;
            EnableGhostFearIfNeed(ghostDefinition);

            foreach (var i in fearStateGhosts) {
                var ghostEntity = fearStateGhosts.GetEntity(i);
                ref var ghostComponent = ref fearStateGhosts.Get1(i);
                ref var fearState = ref fearStateGhosts.Get2(i);

                fearState.estimateTime -= Time.deltaTime;
                if (fearState.estimateTime <= 0) {
                    RemoveFearState(ghostEntity, ghostDefinition, ghostComponent);
                    return;
                }

                var currentPosition = ghostEntity.Get<PositionComponent>().position;
                foreach (var entity in worldService.GetEntitiesOn(currentPosition)) {
                    if (!entity.Has<PlayerComponent>()) continue;

                    entity.Get<PlayerComponent>().scores += ghostDefinition.scoresPerGhost;
                    ghostEntity.Get<WorldObjectDestroyedEvent>();
                    ecsWorld.NewEntity().Get<ScoreTableNeedUpdateEvent>();
                }
            }
        }

        private void EnableGhostFearIfNeed(GhostDefinition ghostDefinition) {
            if (requests.IsEmpty()) return;

            foreach (var i in ghosts) {
                var ghost = ghosts.Get1(i);
                var ghostEntity = ghosts.GetEntity(i);

                ghostEntity.Get<GhostInFearStateComponent>().estimateTime = ghostDefinition.fearStateInSec;
                ghost.renderer.material.color = ghostDefinition.fearState;
            }
        }

        private static void RemoveFearState(EcsEntity ghostEntity, GhostDefinition ghostDefinition, in GhostComponent ghostComponent) {
            ghostEntity.Del<GhostInFearStateComponent>();
            switch (ghostComponent.ghostType) {
                case GhostTypes.Blinky:
                    ghostComponent.renderer.material.color = ghostDefinition.blinky;
                    break;
                case GhostTypes.Pinky:
                    ghostComponent.renderer.material.color = ghostDefinition.pinky;
                    break;
                case GhostTypes.Inky:
                    ghostComponent.renderer.material.color = ghostDefinition.inky;
                    break;
                case GhostTypes.Clyde:
                    ghostComponent.renderer.material.color = ghostDefinition.clyde;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}