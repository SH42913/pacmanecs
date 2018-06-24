using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class WallInitSystem : IEcsInitSystem
    {
        private EcsWorld EcsWorld { get; set; }
        
        public void Initialize()
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

            foreach (GameObject wall in walls)
            {
                var entity = wall.CreateEntityWithPosition(EcsWorld);
                EcsWorld.AddComponent<WallComponent>(entity);
            }
        }

        public void Destroy() {}
    }
}