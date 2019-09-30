using System;
using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;

namespace Players.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void Init()
        {
            if (!_mainGameConfig.PlayerConfig)
            {
                throw new Exception($"{nameof(PlayerConfig)} doesn't exists!");
            }

            int playerCount = 0;
            PlayerConfig playerConfig = _mainGameConfig.PlayerConfig;
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playerObjects)
            {
                EcsEntity playerEntity = _ecsWorld.NewEntityWith(
                    out PlayerComponent playerComponent,
                    out MoveComponent moveComponent);

                Vector2Int startPosition = player.transform.position.ToVector2Int();
                moveComponent.Heading = playerCount % 2 != 0
                    ? Directions.Right
                    : Directions.Left;

                moveComponent.DesiredPosition = startPosition;
                moveComponent.Speed = playerConfig.StartSpeed;

                playerComponent.Lives = playerConfig.StartLives;
                playerComponent.Num = ++playerCount;
                playerComponent.SpawnPosition = startPosition;

                playerEntity.Set<CreateWorldObjectEvent>().Transform = player.transform;
            }
        }
    }
}