using Leopotam.Ecs;
using UnityEngine;
using World;

namespace Walls.Systems
{
    [EcsInject]
    public class WallInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Initialize()
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls)
            {
                _ecsWorld.CreateEntityWith(
                    out CreateWorldObjectEvent createEvent,
                    out WallComponent _);
                
                createEvent.Transform = wall.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}