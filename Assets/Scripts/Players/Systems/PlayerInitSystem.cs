using System;
using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;
using Object = UnityEngine.Object;

namespace Players.Systems
{
    [EcsInject]
    public class PlayerInitSystem : IEcsPreInitSystem, IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<PlayerConfigComponent> _playerConfig = null;

        public void PreInitialize()
        {
            var playerConfigBehaviour = Object.FindObjectOfType<PlayerConfigBehaviour>();
            if (playerConfigBehaviour == null)
            {
                throw new Exception("PlayerConfigBehaviour must be created!");
            }

            if (_world.EntitiesCount <= 0)
            {
                throw new Exception("World must be init!");
            }

            int worldEntity = _world.Entities[0];
            var config = _ecsWorld.AddComponent<PlayerConfigComponent>(worldEntity);
            config.StartSpeed = playerConfigBehaviour.StartSpeed;
            config.StartLives = playerConfigBehaviour.StartLives;
            config.PlayerCount = 0;
        }

        public void Initialize()
        {
            PlayerConfigComponent playerConfig = _playerConfig.Components1[0];
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in playerObjects)
            {
                PlayerComponent playerComponent;
                MoveComponent moveComponent;
                int playerEntity = _ecsWorld.CreateEntityWith(out playerComponent, out moveComponent);

                Vector2Int startPosition = player.transform.position.ToVector2Int();
                moveComponent.Heading = playerConfig.PlayerCount % 2 != 0
                    ? Directions.RIGHT
                    : Directions.LEFT;
                moveComponent.DesiredPosition = startPosition;
                moveComponent.Speed = playerConfig.StartSpeed;

                playerComponent.Lives = playerConfig.StartLives;
                playerComponent.Num = ++playerConfig.PlayerCount;
                playerComponent.StartPosition = startPosition;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(playerEntity).Transform = player.transform;
            }
        }

        public void Destroy()
        {
        }

        public void PreDestroy()
        {
        }
    }
}