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
                var entity = EcsWorld.CreateEntity();
                EcsWorld.AddComponent<WallComponent>(entity);
                EcsWorld
                    .AddComponent<PositionComponent>(entity)
                    .Position = wall.transform.position.ToVector2Int();
            }
        }

        public void Destroy() {}
    }
}