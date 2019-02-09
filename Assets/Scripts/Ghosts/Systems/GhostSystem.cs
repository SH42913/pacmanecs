using Death;
using Leopotam.Ecs;
using Moving;
using Players;
using UnityEngine;
using World;
using Random = System.Random;

namespace Ghosts.Systems
{
    [EcsInject]
    public class GhostSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<GhostComponent, StoppedComponent> _stoppedGhosts = null;
        private readonly EcsFilter<PositionComponent, GhostComponent>.Exclude<GhostInFearStateComponent> _ghosts = null;

        private static readonly Random Random = new Random();

        public void Run()
        {
            WorldComponent world = _world.Components1[0];
            foreach (int i in _stoppedGhosts)
            {
                int ghostEntity = _stoppedGhosts.Entities[i];
                _ecsWorld
                    .AddComponent<ChangeDirectionComponent>(ghostEntity)
                    .NewDirection = Random.NextEnum<Directions>();
            }

            foreach (int i in _ghosts)
            {
                Vector2Int currentPosition = _ghosts.Components1[i].Position;
                foreach (int entity in world.WorldField[currentPosition.x][currentPosition.y])
                {
                    var player = _ecsWorld.GetComponent<PlayerComponent>(entity);
                    var isDead = _ecsWorld.GetComponent<PlayerIsDeadEvent>(entity);
                    if (player != null && isDead == null)
                    {
                        _ecsWorld.AddComponent<PlayerIsDeadEvent>(entity);
                    }
                }
            }
        }
    }
}