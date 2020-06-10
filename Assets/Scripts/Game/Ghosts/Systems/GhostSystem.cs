using Utils;
using Game.Death;
using Game.Moving;
using Game.Players;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;
using Random = System.Random;

namespace Game.Ghosts {
    public sealed class GhostSystem : IEcsRunSystem {
        private readonly Random random = null;
        private readonly WorldService worldService = null;

        private readonly EcsFilter<GhostComponent, StoppedComponent> stoppedGhosts = null;
        private readonly EcsFilter<PositionComponent, GhostComponent>.Exclude<GhostInFearStateComponent> ghosts = null;

        public void Run() {
            foreach (var i in stoppedGhosts) {
                stoppedGhosts.GetEntity(i).Get<ChangeDirectionEvent>().newDirection = random.NextEnum<Directions>();
            }

            foreach (var i in ghosts) {
                var currentPosition = ghosts.Get1(i).position;
                foreach (var entity in worldService.worldField[currentPosition.x][currentPosition.y]) {
                    if (entity.Has<PlayerComponent>() && !entity.Has<PlayerIsDeadEvent>()) {
                        entity.Get<PlayerIsDeadEvent>();
                    }
                }
            }
        }
    }
}