using Game.Gameplay.Players;
using Game.Gameplay.World;
using Leopotam.Ecs;

namespace Game.Gameplay.Items {
    public sealed class ItemSystem : IEcsRunSystem {
        private readonly WorldService worldService = null;
        private readonly EcsFilter<WorldObjectNewPositionRequest, PlayerComponent> players = null;

        public void Run() {
            foreach (var i in players) {
                var newPosition = players.Get1(i).newPosition;
                var playerEntity = players.GetEntity(i);

                foreach (var entity in worldService.GetEntitiesOn(newPosition)) {
                    if (!entity.Has<ItemMarker>()) continue;

                    entity.Get<ItemTakenEvent>().playerEntity = playerEntity;
                    entity.Replace(new WorldObjectDestroyedEvent {
                        deleteEntity = true,
                    });
                }
            }
        }
    }
}