using System;
using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Players.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void Init()
        {
            if (!_gameDefinitions.playerDefinition)
            {
                throw new Exception($"{nameof(PlayerDefinition)} doesn't exists!");
            }

            int playerCount = 0;
            PlayerDefinition playerDefinition = _gameDefinitions.playerDefinition;
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
                moveComponent.Speed = playerDefinition.StartSpeed;

                playerComponent.Lives = playerDefinition.StartLives;
                playerComponent.Num = ++playerCount;
                playerComponent.SpawnPosition = startPosition;

                playerEntity.Set<CreateWorldObjectEvent>().Transform = player.transform;
            }
        }
    }
}