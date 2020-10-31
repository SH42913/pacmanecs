using Game.Gameplay.World;
using Leopotam.Ecs;

namespace Game.Gameplay.Walls {
    public sealed class WallInitSystem : IEcsInitSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly WallRegistry registry = null;

        public void Init() {
            foreach (var wall in registry.walls) {
                ecsWorld.NewEntity()
                    .Replace(new WallMarker())
                    .Replace(new WorldObjectCreateRequest {
                        transform = wall.transform,
                    });
            }
        }
    }
}