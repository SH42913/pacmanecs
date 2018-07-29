using Components.BaseComponents;
using Components.PlayerComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class PlayerInputSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<PlayerComponent> _players = null;
        
        public void Run()
        {
            for (int i = 0; i < _players.EntitiesCount; i++)
            {
                int playerNum = _players.Components1[i].Num;
                float yAxis = Input.GetAxis(string.Format("Player{0}Y", playerNum));
                float xAxis = Input.GetAxis(string.Format("Player{0}X", playerNum));

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

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    _ecsWorld.CreateEntityWith<GameStateComponent>().State = Time.timeScale < 1
                        ? GameStates.START
                        : GameStates.PAUSE;
                }
            }
        }

        private void SendCommand(Directions newDirection, int playerNum)
        {
            var command = _ecsWorld.CreateEntityWith<CommandComponent>();
            command.PlayerNum = playerNum;
            command.NewDirection = newDirection;
        }
    }
}