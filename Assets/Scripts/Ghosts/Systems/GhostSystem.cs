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
        private readonly EcsFilter<PositionComponent, GhostComponent> _ghosts = null;

        private static readonly Random Random = new Random();

        public void Run()
        {
            WorldComponent world = _world.Components1[0];
            foreach (int i in _ghosts)
            {
                Vector2Int currentPosition = _ghosts.Components1[i].Position;
                int ghostEntity = _ghosts.Entities[i];

                foreach (int entity in world.WorldField[currentPosition.x][currentPosition.y])
                {
                    var player = _ecsWorld.GetComponent<PlayerComponent>(entity);
                    if (player != null)
                    {
                        _ecsWorld.AddComponent<PlayerIsDeadEvent>(entity);
                    }
                }

                var stopped = _ecsWorld.GetComponent<StoppedComponent>(ghostEntity);
                if (stopped != null)
                {
                    _ecsWorld
                        .AddComponent<ChangeDirectionComponent>(ghostEntity)
                        .NewDirection = Random.NextEnum<Directions>();
                }
            }
        }
    }
}