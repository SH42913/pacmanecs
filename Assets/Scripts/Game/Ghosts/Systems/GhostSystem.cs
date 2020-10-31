using Game.Moving;
using Game.Players;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;
using Utils;

namespace Game.Ghosts {
    public sealed class GhostSystem : IEcsRunSystem {
        private readonly System.Random random = null;
        private readonly WorldService worldService = null;
        private readonly EcsFilter<PositionComponent, MovementComponent, GhostComponent> ghosts = null;

        public void Run() {
            foreach (var i in ghosts) {
                ref var ghostEntity = ref ghosts.GetEntity(i);

                var position = ghosts.Get1(i).position;
                var isFear = ghostEntity.Has<GhostInFearStateComponent>();
                if (!isFear && TryGetAlivePlayerOnPosition(position, out var playerEntity)) {
                    playerEntity.Replace(new PlayerDeathRequest());
                }

                if (ghostEntity.Has<MovementStoppedMarker>()) {
                    ref var movement = ref ghosts.Get2(i);
                    ChangeDirection(ref movement);
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

        private void ChangeDirection(ref MovementComponent movement) {
            movement.heading = random.NextEnum<Directions>();
        }
    }
}