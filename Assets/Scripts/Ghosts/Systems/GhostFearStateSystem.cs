using System;
using Leopotam.Ecs;
using Players;
using Ui.ScoreTable;
using UnityEngine;
using World;

namespace Ghosts.Systems
{
    [EcsInject]
    public class GhostFearStateSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;
        
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<EnableGhostFearStateEvent> _enableEvents = null;
        private readonly EcsFilter<GhostComponent, WorldObjectComponent> _ghosts = null;
        private readonly EcsFilter<GhostComponent, GhostInFearStateComponent, WorldObjectComponent> _fearStateGhosts = null;

        public void Run()
        {
            GhostConfig ghostConfig = _mainGameConfig.GhostConfig;
            WorldComponent world = _world.Components1[0];

            if (!_enableEvents.IsEmpty())
            {
                foreach (int i in _ghosts)
                {
                    GameObject ghost = _ghosts.Components2[i].Transform.gameObject;
                    EcsEntity ghostEntity = _ghosts.Entities[i];

                    _ecsWorld
                        .EnsureComponent<GhostInFearStateComponent>(ghostEntity, out bool isNew)
                        .EstimateTime = ghostConfig.FearStateInSec;

                    if (isNew)
                    {
                        ghost.GetComponent<MeshRenderer>().material.color = ghostConfig.FearState;
                    }
                }
            }

            foreach (int i in _fearStateGhosts)
            {
                GhostComponent ghostComponent = _fearStateGhosts.Components1[i];
                GhostInFearStateComponent fearState = _fearStateGhosts.Components2[i];
                GameObject ghost = _fearStateGhosts.Components3[i].Transform.gameObject;
                EcsEntity ghostEntity = _fearStateGhosts.Entities[i];

                fearState.EstimateTime -= Time.deltaTime;
                if (fearState.EstimateTime <= 0)
                {
                    _ecsWorld.RemoveComponent<GhostInFearStateComponent>(ghostEntity);

                    var material = ghost.GetComponent<MeshRenderer>().material;
                    switch (ghostComponent.GhostType)
                    {
                        case GhostTypes.BLINKY:
                            material.color = ghostConfig.Blinky;
                            break;
                        case GhostTypes.PINKY:
                            material.color = ghostConfig.Pinky;
                            break;
                        case GhostTypes.INKY:
                            material.color = ghostConfig.Inky;
                            break;
                        case GhostTypes.CLYDE:
                            material.color = ghostConfig.Clyde;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    return;
                }

                Vector2Int currentPosition = _ecsWorld.GetComponent<PositionComponent>(ghostEntity).Position;
                foreach (EcsEntity entity in world.WorldField[currentPosition.x][currentPosition.y])
                {
                    var player = _ecsWorld.GetComponent<PlayerComponent>(entity);
                    if (player == null) continue;

                    player.Scores += ghostConfig.ScoresPerGhost;
                    _ecsWorld.AddComponent<DestroyedWorldObjectComponent>(ghostEntity);
                    _ecsWorld.CreateEntityWith(out UpdateScoreTableEvent _);
                }
            }
        }
    }
}