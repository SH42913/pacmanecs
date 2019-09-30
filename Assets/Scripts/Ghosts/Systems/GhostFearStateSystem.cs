using System;
using Leopotam.Ecs;
using Players;
using Ui.ScoreTable;
using UnityEngine;
using World;

namespace Ghosts.Systems
{
    public class GhostFearStateSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<EnableGhostFearStateEvent> _enableEvents = null;
        private readonly EcsFilter<GhostComponent> _ghosts = null;
        private readonly EcsFilter<GhostComponent, GhostInFearStateComponent> _fearStateGhosts = null;

        public void Run()
        {
            GhostConfig ghostConfig = _mainGameConfig.GhostConfig;
            WorldComponent world = _world.Get1[0];

            if (!_enableEvents.IsEmpty())
            {
                foreach (int i in _ghosts)
                {
                    GhostComponent ghost = _ghosts.Get1[i];
                    EcsEntity ghostEntity = _ghosts.Entities[i];

                    ghostEntity.Set<GhostInFearStateComponent>().EstimateTime = ghostConfig.FearStateInSec;
                    ghost.Renderer.material.color = ghostConfig.FearState;
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
                            ghostComponent.Renderer.material.color = ghostConfig.Blinky;
                            break;
                        case GhostTypes.Pinky:
                            ghostComponent.Renderer.material.color = ghostConfig.Pinky;
                            break;
                        case GhostTypes.Inky:
                            ghostComponent.Renderer.material.color = ghostConfig.Inky;
                            break;
                        case GhostTypes.Clyde:
                            ghostComponent.Renderer.material.color = ghostConfig.Clyde;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    return;
                }

                Vector2Int currentPosition = ghostEntity.Set<PositionComponent>().Position;
                foreach (EcsEntity entity in world.WorldField[currentPosition.x][currentPosition.y])
                {
                    var player = entity.Get<PlayerComponent>();
                    if (player == null) continue;

                    player.Scores += ghostConfig.ScoresPerGhost;
                    ghostEntity.Set<DestroyedWorldObjectComponent>();
                    _ecsWorld.NewEntityWith(out UpdateScoreTableEvent _);
                }
            }
        }
    }
}