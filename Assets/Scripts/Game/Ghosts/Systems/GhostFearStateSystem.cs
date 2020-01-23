using System;
using Game.Players;
using Game.Ui.ScoreTable;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ghosts
{
    public class GhostFearStateSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly WorldService _worldService = null;
        private readonly GameDefinitions _gameDefinitions = null;

        private readonly EcsFilter<EnableGhostFearStateEvent> _enableEvents = null;
        private readonly EcsFilter<GhostComponent> _ghosts = null;
        private readonly EcsFilter<GhostComponent, GhostInFearStateComponent> _fearStateGhosts = null;

        public void Run()
        {
            GhostDefinition ghostDefinition = _gameDefinitions.ghostDefinition;
            if (!_enableEvents.IsEmpty())
            {
                foreach (int i in _ghosts)
                {
                    GhostComponent ghost = _ghosts.Get1[i];
                    EcsEntity ghostEntity = _ghosts.Entities[i];

                    ghostEntity.Set<GhostInFearStateComponent>().EstimateTime = ghostDefinition.FearStateInSec;
                    ghost.Renderer.material.color = ghostDefinition.FearState;
                }
            }

            foreach (int i in _fearStateGhosts)
            {
                GhostComponent ghostComponent = _fearStateGhosts.Get1[i];
                GhostInFearStateComponent fearState = _fearStateGhosts.Get2[i];
                EcsEntity ghostEntity = _fearStateGhosts.Entities[i];

                fearState.EstimateTime -= Time.deltaTime;
                if (fearState.EstimateTime <= 0)
                {
                    ghostEntity.Unset<GhostInFearStateComponent>();
                    switch (ghostComponent.GhostType)
                    {
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

                    return;
                }

                Vector2Int currentPosition = ghostEntity.Set<PositionComponent>().Position;
                foreach (EcsEntity entity in _worldService.WorldField[currentPosition.x][currentPosition.y])
                {
                    var player = entity.Get<PlayerComponent>();
                    if (player == null) continue;

                    player.Scores += ghostDefinition.ScoresPerGhost;
                    ghostEntity.Set<DestroyedWorldObjectEvent>();
                    _ecsWorld.NewEntityWith(out UpdateScoreTableEvent _);
                }
            }
        }
    }
}