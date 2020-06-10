using System;
using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Players {
    public class PlayerInitSystem : IEcsInitSystem {
        private readonly EcsWorld _ecsWorld = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void Init() {
            if (!_gameDefinitions.playerDefinition) {
                throw new Exception($"{nameof(PlayerDefinition)} doesn't exists!");
            }

            int playerCount = 0;
            PlayerDefinition playerDefinition = _gameDefinitions.playerDefinition;
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playerObjects) {
                Vector2Int startPosition = player.transform.position.ToVector2Int();
                var startHeading = playerCount % 2 != 0
                    ? Directions.Right
                    : Directions.Left;

                EcsEntity playerEntity = _ecsWorld.NewEntity();
                playerEntity
                    .Replace(new CreateWorldObjectEvent {
                        Transform = player.transform
                    })
                    .Replace(new PlayerComponent {
                        Lives = playerDefinition.StartLives,
                        Num = ++playerCount,
                        SpawnPosition = startPosition,
                    })
                    .Replace(new MoveComponent {
                        Heading = startHeading,
                        DesiredPosition = startPosition,
                        Speed = playerDefinition.StartSpeed,
                    });
            }
        }
    }
}