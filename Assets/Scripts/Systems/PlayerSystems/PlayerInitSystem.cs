using Components.BaseComponents;
using Components.PlayerComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class PlayerInitSystem : IEcsInitSystem
    {
        public int StartLifes;
        public float StartSpeed;

        private EcsWorld _ecsWorld = null;
        private int _createdPlayersCount;

        public void Initialize()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in playerObjects)
            {
                var entity = player.CreateEntityWithPosition(_ecsWorld);
                var startPosition = player.transform.position;

                var moveComponent = _ecsWorld.AddComponent<MoveComponent>(entity);
                moveComponent.Transform = player.transform;
                moveComponent.Heading = _createdPlayersCount % 2 != 0
                    ? Directions.RIGHT
                    : Directions.LEFT;
                moveComponent.DesiredPosition = startPosition;
                moveComponent.Speed = StartSpeed;

                var playerComponent = _ecsWorld.AddComponent<PlayerComponent>(entity);
                playerComponent.Lifes = StartLifes;
                playerComponent.Num = ++_createdPlayersCount;
                playerComponent.StartPosition = startPosition;
            }
        }

        public void Destroy() 
        {}
    }
}