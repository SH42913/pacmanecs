using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Walls {
    public class WallInitSystem : IEcsInitSystem {
        private readonly EcsWorld _ecsWorld = null;

        public void Init() {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls) {
                var entity = _ecsWorld.NewEntity();
                entity.Set<CreateWorldObjectEvent>().Transform = wall.transform;
                entity.Set<WallComponent>();
            }
        }
    }
}