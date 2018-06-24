using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class PlayerSystem : IEcsInitSystem, IEcsRunSystem
    {
        public uint StartLifes { get; set; }
        public uint CreatedPlayersCount { get; private set; }
        
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<MoveComponent, PlayerComponent> Players { get; set; }

        public void Initialize()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in playerObjects)
            {
                var entity = EcsWorld.CreateEntity();
                var startPosition = player.transform.position;
            
                EcsWorld
                    .AddComponent<PositionComponent>(entity)
                    .Position = startPosition.ToVector2Int();

                var moveComponent = EcsWorld.AddComponent<MoveComponent>(entity);
                moveComponent.Transform = player.transform;
                moveComponent.Heading = CreatedPlayersCount % 2 != 0
                    ? Directions.RIGHT
                    : Directions.LEFT;
                moveComponent.DesiredPosition = startPosition;
                moveComponent.Speed = 3f;

                var playerComponent = EcsWorld.AddComponent<PlayerComponent>(entity);
                playerComponent.Lifes = StartLifes;
                playerComponent.Num = ++CreatedPlayersCount;
                playerComponent.StartPosition = startPosition;
            }
        }
        
        public void Run()
        {
            
        }

        public void Destroy() {}
    }
}