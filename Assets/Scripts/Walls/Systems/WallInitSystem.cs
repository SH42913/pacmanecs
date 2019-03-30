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
                int wallEntity = _ecsWorld.CreateEntityWith(out WallComponent _);
                _ecsWorld.AddComponent<CreateWorldObjectEvent>(wallEntity).Transform = wall.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}