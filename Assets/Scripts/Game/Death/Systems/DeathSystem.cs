using Game.Players;
using Game.Teleports;
using Game.Ui.ScoreTable;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Death {
    public class DeathSystem : IEcsRunSystem {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<PlayerComponent, PlayerIsDeadEvent> _deadPlayers = null;

        public void Run() {
            foreach (int i in _deadPlayers) {
                EcsEntity playerEntity = _deadPlayers.GetEntity(i);
                ref PlayerComponent deadPlayer = ref _deadPlayers.Get1(i);

                Vector2Int spawnPosition = deadPlayer.SpawnPosition;
                if (--deadPlayer.Lives <= 0) {
                    deadPlayer.IsDead = true;
                    spawnPosition = Vector2Int.zero;
                    playerEntity.Set<DestroyedWorldObjectEvent>();
                }

                playerEntity.Set<TeleportedEvent>().NewPosition = spawnPosition;
                _ecsWorld.NewEntity().Set<UpdateScoreTableEvent>();
            }
        }
    }
}