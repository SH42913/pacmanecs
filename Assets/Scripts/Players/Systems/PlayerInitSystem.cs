using System;
using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;

namespace Players.Systems
{
    [EcsInject]
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void Initialize()
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
                int playerEntity = _ecsWorld.CreateEntityWith(
                    out PlayerComponent playerComponent, 
                    out MoveComponent moveComponent);

                Vector2Int startPosition = player.transform.position.ToVector2Int();
                moveComponent.Heading = playerCount % 2 != 0
                    ? Directions.RIGHT
                    : Directions.LEFT;
                moveComponent.DesiredPosition = startPosition;
                moveComponent.Speed = playerConfig.StartSpeed;

                playerComponent.Lives = playerConfig.StartLives;
                playerComponent.Num = ++playerCount;
                playerComponent.StartPosition = startPosition;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(playerEntity).Transform = player.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}