using Game.Players;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Items {
    public sealed class ItemSystem : IEcsRunSystem {
        private readonly WorldService worldService = null;
        private readonly EcsFilter<NewPositionEvent, PlayerComponent> players = null;

        public void Run() {
            foreach (var i in players) {
                var newPosition = players.Get1(i).newPosition;
                var playerEntity = players.GetEntity(i);

                foreach (var entity in worldService.worldField[newPosition.x][newPosition.y]) {
                    if (!entity.Has<ItemComponent>()) continue;

                    entity.Get<TakenItemEvent>().playerEntity = playerEntity;
                    entity.Get<DestroyedWorldObjectEvent>();
                }
            }
        }
    }
}