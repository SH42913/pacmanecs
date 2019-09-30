using Leopotam.Ecs;
using UnityEngine;
using World;

namespace Walls.Systems
{
    public class WallInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Init()
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls)
            {
                _ecsWorld.NewEntityWith(
                    out CreateWorldObjectEvent createEvent,
                    out WallComponent _);

                createEvent.Transform = wall.transform;
            }
        }
    }
}