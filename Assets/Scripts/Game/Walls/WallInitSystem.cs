using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Walls {
    public sealed class WallInitSystem : IEcsInitSystem {
        private readonly EcsWorld ecsWorld = null;

        public void Init() {
            var walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (var wall in walls) {
                var entity = ecsWorld.NewEntity();
                entity.Get<WorldObjectCreateRequest>().transform = wall.transform;
                entity.Get<WallMarker>();
            }
        }
    }
}