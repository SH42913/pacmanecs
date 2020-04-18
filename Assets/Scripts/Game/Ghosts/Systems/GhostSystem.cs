using Utils;
using Game.Death;
using Game.Moving;
using Game.Players;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;
using Random = System.Random;

namespace Game.Ghosts {
    public class GhostSystem : IEcsRunSystem {
        private readonly Random _random = null;
        private readonly WorldService _worldService = null;

        private readonly EcsFilter<GhostComponent, StoppedComponent> _stoppedGhosts = null;
        private readonly EcsFilter<PositionComponent, GhostComponent>.Exclude<GhostInFearStateComponent> _ghosts = null;

        public void Run() {
            foreach (int i in _stoppedGhosts) {
                EcsEntity ghostEntity = _stoppedGhosts.GetEntity(i);
                ghostEntity.Set<ChangeDirectionEvent>().NewDirection = _random.NextEnum<Directions>();
            }

            foreach (int i in _ghosts) {
                Vector2Int currentPosition = _ghosts.Get1(i).Position;
                foreach (EcsEntity entity in _worldService.WorldField[currentPosition.x][currentPosition.y]) {
                    if (entity.Has<PlayerComponent>() && !entity.Has<PlayerIsDeadEvent>()) {
                        entity.Set<PlayerIsDeadEvent>();
                    }
                }
            }
        }
    }
}