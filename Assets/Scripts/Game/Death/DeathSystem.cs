using Game.Players;
using Game.Teleports;
using Game.Ui.ScoreTable;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Death {
    public sealed class DeathSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<PlayerComponent, PlayerIsDeadEvent> deadPlayers = null;

        public void Run() {
            foreach (var i in deadPlayers) {
                var playerEntity = deadPlayers.GetEntity(i);
                ref var deadPlayer = ref deadPlayers.Get1(i);

                var spawnPosition = deadPlayer.spawnPosition;
                if (--deadPlayer.lives <= 0) {
                    deadPlayer.isDead = true;
                    spawnPosition = Vector2Int.zero;
                    playerEntity.Get<DestroyedWorldObjectEvent>();
                }

                playerEntity.Get<TeleportedEvent>().newPosition = spawnPosition;
                ecsWorld.NewEntity().Get<UpdateScoreTableEvent>();
            }
        }
    }
}