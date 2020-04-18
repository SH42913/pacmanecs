using System;
using Game.Players;
using Game.Ui.ScoreTable;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ghosts {
    public class GhostFearStateSystem : IEcsRunSystem {
        private readonly EcsWorld _ecsWorld = null;
        private readonly WorldService _worldService = null;
        private readonly GameDefinitions _gameDefinitions = null;

        private readonly EcsFilter<EnableGhostFearStateEvent> _enableEvents = null;
        private readonly EcsFilter<GhostComponent> _ghosts = null;
        private readonly EcsFilter<GhostComponent, GhostInFearStateComponent> _fearStateGhosts = null;

        public void Run() {
            GhostDefinition ghostDefinition = _gameDefinitions.ghostDefinition;
            EnableGhostFearIfNeed(ghostDefinition);

            foreach (int i in _fearStateGhosts) {
                EcsEntity ghostEntity = _fearStateGhosts.GetEntity(i);
                ref GhostComponent ghostComponent = ref _fearStateGhosts.Get1(i);
                ref GhostInFearStateComponent fearState = ref _fearStateGhosts.Get2(i);

                fearState.EstimateTime -= Time.deltaTime;
                if (fearState.EstimateTime <= 0) {
                    RemoveFearState(ghostEntity, ghostDefinition, ghostComponent);
                    return;
                }

                Vector2Int currentPosition = ghostEntity.Set<PositionComponent>().Position;
                foreach (EcsEntity entity in _worldService.WorldField[currentPosition.x][currentPosition.y]) {
                    if (!entity.Has<PlayerComponent>()) continue;

                    entity.Set<PlayerComponent>().Scores += ghostDefinition.ScoresPerGhost;
                    ghostEntity.Set<DestroyedWorldObjectEvent>();
                    _ecsWorld.NewEntity().Set<UpdateScoreTableEvent>();
                }
            }
        }

        private void EnableGhostFearIfNeed(GhostDefinition ghostDefinition) {
            if (_enableEvents.IsEmpty()) return;

            foreach (int i in _ghosts) {
                GhostComponent ghost = _ghosts.Get1(i);
                EcsEntity ghostEntity = _ghosts.GetEntity(i);

                ghostEntity.Set<GhostInFearStateComponent>().EstimateTime = ghostDefinition.FearStateInSec;
                ghost.Renderer.material.color = ghostDefinition.FearState;
            }
        }

        private static void RemoveFearState(EcsEntity ghostEntity, GhostDefinition ghostDefinition, in GhostComponent ghostComponent) {
            ghostEntity.Unset<GhostInFearStateComponent>();
            switch (ghostComponent.GhostType) {
                case GhostTypes.Blinky:
                    ghostComponent.Renderer.material.color = ghostDefinition.Blinky;
                    break;
                case GhostTypes.Pinky:
                    ghostComponent.Renderer.material.color = ghostDefinition.Pinky;
                    break;
                case GhostTypes.Inky:
                    ghostComponent.Renderer.material.color = ghostDefinition.Inky;
                    break;
                case GhostTypes.Clyde:
                    ghostComponent.Renderer.material.color = ghostDefinition.Clyde;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}