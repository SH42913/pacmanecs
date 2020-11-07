using System;
using Game.Gameplay.Movement;
using Game.Gameplay.Players;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Ghosts {
    public sealed class GhostSystem : IEcsRunSystem {
        private readonly System.Random random = null;
        private readonly WorldService worldService = null;
        private readonly GameDefinitions gameDefinitions = null;

        private readonly EcsFilter<PositionComponent, MovementComponent, PlayerComponent> players = null;
        private readonly EcsFilter<PositionComponent, MovementComponent, GhostComponent> ghosts = null;

        public void Run() {
            var playerPosition = default(Vector2Int);
            var playerDirection = default(Directions);
            if (!players.IsEmpty()) {
                playerPosition = players.Get1(0).position;
                playerDirection = players.Get2(0).heading;
            }

            foreach (var i in ghosts) {
                ref var movement = ref ghosts.Get2(i);
                ref var ghostEntity = ref ghosts.GetEntity(i);
                var currentPosition = ghosts.Get1(i).position;

                if (ghostEntity.Has<GhostInFearStateComponent>()) {
                    ghostEntity.Del<MovementTargetComponent>();
                    if (ghostEntity.Has<MovementStoppedMarker>()) {
                        movement.heading = random.NextEnum<Directions>();
                    }
                } else {
                    ref var ghostComponent = ref ghosts.Get3(i);
                    ghostEntity.Replace(new MovementTargetComponent {
                        target = GetGhostTarget(ghostComponent.ghostType, currentPosition, playerPosition, playerDirection),
                    });

                    if (TryGetAlivePlayerOnPosition(currentPosition, out var playerEntity)) {
                        playerEntity.Replace(new PlayerDeathRequest());
                    }
                }
            }
        }

        private bool TryGetAlivePlayerOnPosition(in Vector2Int currentPosition, out EcsEntity playerEntity) {
            playerEntity = EcsEntity.Null;

            foreach (var entity in worldService.GetEntitiesOn(currentPosition)) {
                if (entity.Has<PlayerComponent>() && !entity.Has<PlayerDeathRequest>()) {
                    playerEntity = entity;
                    return true;
                }
            }

            return false;
        }

        private Vector2Int GetGhostTarget(GhostTypes ghostType, in Vector2Int currentPosition, in Vector2Int playerPosition, Directions playerDirection) {
            switch (ghostType) {
                case GhostTypes.Blinky: return playerPosition;
                case GhostTypes.Pinky: return playerPosition + playerDirection.GetPosition() * 6;

                case GhostTypes.Inky: {
                    var distanceToPlayer = currentPosition - playerPosition;
                    if (distanceToPlayer.sqrMagnitude > 8 * 8) {
                        return gameDefinitions.worldDefinition.worldSize;
                    }

                    return playerPosition;
                }

                case GhostTypes.Clyde: {
                    var distanceToPlayer = currentPosition - playerPosition;
                    if (distanceToPlayer.sqrMagnitude > 8 * 8) {
                        return playerPosition;
                    }

                    return -Vector2Int.one;
                }

                default: throw new ArgumentOutOfRangeException(nameof(ghostType), ghostType, null);
            }
        }
    }
}