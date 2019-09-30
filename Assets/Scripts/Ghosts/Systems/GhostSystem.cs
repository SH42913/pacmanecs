using Death;
using Leopotam.Ecs;
using Moving;
using Players;
using UnityEngine;
using World;
using Random = System.Random;

namespace Ghosts.Systems
{
    public class GhostSystem : IEcsRunSystem
    {
        private readonly Random _random = null;
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<GhostComponent, StoppedComponent> _stoppedGhosts = null;
        private readonly EcsFilter<PositionComponent, GhostComponent>.Exclude<GhostInFearStateComponent> _ghosts = null;

        public void Run()
        {
            WorldComponent world = _world.Get1[0];
            foreach (int i in _stoppedGhosts)
            {
                EcsEntity ghostEntity = _stoppedGhosts.Entities[i];
                ghostEntity.Set<ChangeDirectionComponent>().NewDirection = _random.NextEnum<Directions>();
            }

            foreach (int i in _ghosts)
            {
                Vector2Int currentPosition = _ghosts.Get1[i].Position;
                foreach (EcsEntity entity in world.WorldField[currentPosition.x][currentPosition.y])
                {
                    var player = entity.Get<PlayerComponent>();
                    var isDead = entity.Get<PlayerIsDeadEvent>();
                    if (player != null && isDead == null)
                    {
                        entity.Set<PlayerIsDeadEvent>();
                    }
                }
            }
        }
    }
}