using Game.Gameplay.Teleports;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Players {
    public sealed class PlayerDeathSystem : IEcsRunSystem {
        private readonly EcsFilter<PlayerComponent, PlayerDeathRequest> requests = null;

        public void Run() {
            foreach (var i in requests) {
                var playerEntity = requests.GetEntity(i);
                ref var deadPlayer = ref requests.Get1(i);

                var spawnPosition = deadPlayer.spawnPosition;
                if (--deadPlayer.lives <= 0) {
                    spawnPosition = Vector2Int.zero;
                    playerEntity.Get<DeadPlayerMarker>();
                    playerEntity.Get<WorldObjectDestroyedEvent>();
                }

                playerEntity.Get<TeleportToPositionRequest>().newPosition = spawnPosition;
                playerEntity.Get<PlayerScoreChangedEvent>();
            }
        }
    }
}