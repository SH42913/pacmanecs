using Game.Gameplay.Teleports;
using Game.Gameplay.Ui.ScoreTable;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Players {
    public sealed class PlayerDeathSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<PlayerComponent, PlayerDeathRequest> requests = null;

        public void Run() {
            foreach (var i in requests) {
                var playerEntity = requests.GetEntity(i);
                ref var deadPlayer = ref requests.Get1(i);

                var spawnPosition = deadPlayer.spawnPosition;
                if (--deadPlayer.lives <= 0) {
                    deadPlayer.isDead = true;
                    spawnPosition = Vector2Int.zero;
                    playerEntity.Get<WorldObjectDestroyedEvent>();
                }

                playerEntity.Get<TeleportToPositionRequest>().newPosition = spawnPosition;
                ecsWorld.NewEntity().Get<ScoreTableNeedUpdateEvent>();
            }
        }
    }
}