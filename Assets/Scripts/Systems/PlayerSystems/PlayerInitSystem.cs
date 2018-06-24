using Components.BaseComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class PlayerInitSystem : IEcsInitSystem
    {
        public int StartLifes { get; set; }
        public float StartSpeed { get; set; }
        public int CreatedPlayersCount { get; private set; }
        
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<MoveComponent, PlayerComponent> Players { get; set; }

        public void Initialize()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in playerObjects)
            {
                var entity = player.CreateEntityWithPosition(EcsWorld);
                var startPosition = player.transform.position;

                var moveComponent = EcsWorld.AddComponent<MoveComponent>(entity);
                moveComponent.Transform = player.transform;
                moveComponent.Heading = CreatedPlayersCount % 2 != 0
                    ? Directions.RIGHT
                    : Directions.LEFT;
                moveComponent.DesiredPosition = startPosition;
                moveComponent.Speed = StartSpeed;

                var playerComponent = EcsWorld.AddComponent<PlayerComponent>(entity);
                playerComponent.Lifes = StartLifes;
                playerComponent.Num = ++CreatedPlayersCount;
                playerComponent.StartPosition = startPosition;
            }
        }

        public void Destroy() {}
    }
}