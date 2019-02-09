using Leopotam.Ecs;
using Players;
using Teleports;
using Ui.ScoreTable;
using UnityEngine;
using World;

namespace Death.Systems
{
    [EcsInject]
    public class DeathSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<PlayerComponent, PlayerIsDeadEvent> _deadPlayers = null;

        public void Run()
        {
            foreach (int i in _deadPlayers)
            {
                PlayerComponent deadPlayer = _deadPlayers.Components1[i];
                int playerEntity = _deadPlayers.Entities[i];

                Vector2Int spawnPosition = deadPlayer.StartPosition;
                if (--deadPlayer.Lives <= 0)
                {
                    deadPlayer.IsDead = true;
                    spawnPosition = Vector2Int.zero;
                    _ecsWorld.AddComponent<DestroyedWorldObjectComponent>(playerEntity);
                }

                _ecsWorld.AddComponent<TeleportingComponent>(playerEntity).NewPosition = spawnPosition;
                _ecsWorld.CreateEntityWith<UpdateScoreTableEvent>();
            }
        }
    }
}