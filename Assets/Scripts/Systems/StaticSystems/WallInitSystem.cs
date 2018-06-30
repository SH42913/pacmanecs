using Components.StaticComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.StaticSystems
{
    [EcsInject]
    public class WallInitSystem : IEcsInitSystem
    {
        private EcsWorld _ecsWorld = null;
        
        public void Initialize()
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

            foreach (GameObject wall in walls)
            {
                var entity = wall.CreateEntityWithPosition(_ecsWorld);
                _ecsWorld.AddComponent<WallComponent>(entity);
            }
        }

        public void Destroy() {}
    }
}