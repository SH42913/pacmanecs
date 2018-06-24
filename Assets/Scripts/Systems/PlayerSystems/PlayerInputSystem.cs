using Components;
using Components.BaseComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class PlayerInputSystem : IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<PlayerComponent> Players { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < Players.EntitiesCount; i++)
            {
                uint playerNum = Players.Components1[i].Num;
                float yAxis = Input.GetAxis($"Player{playerNum}Y");
                float xAxis = Input.GetAxis($"Player{playerNum}X");

                if (yAxis > 0)
                {
                    SendCommand(Directions.UP, playerNum);
                }
                else if (yAxis < 0)
                {
                    SendCommand(Directions.DOWN, playerNum);
                }
                else if (xAxis > 0)
                {
                    SendCommand(Directions.RIGHT, playerNum);
                }
                else if(xAxis < 0)
                {
                    SendCommand(Directions.LEFT, playerNum);
                }
            }
        }

        private void SendCommand(Directions newDirection, uint playerNum)
        {
            var command = EcsWorld.CreateEntityWith<CommandComponent>();
            command.PlayerNum = playerNum;
            command.NewDirection = newDirection;
        }
    }
}