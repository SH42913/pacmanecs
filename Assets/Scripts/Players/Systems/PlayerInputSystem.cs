using Leopotam.Ecs;
using Moving;
using Ui.GameStates;
using UnityEngine;

namespace Players.Systems
{
    [EcsInject]
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<PlayerComponent> _players = null;

        public void Run()
        {
            foreach (int i in _players)
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
                else if (xAxis < 0)
                {
                    SendCommand(Directions.LEFT, playerNum);
                }

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    _ecsWorld.CreateEntityWith<ChangeGameStateEvent>().State = Time.timeScale < 1
                        ? GameStates.START
                        : GameStates.PAUSE;
                }
            }
        }

        private void SendCommand(Directions newDirection, int playerEntity)
        {
            var command = _ecsWorld.EnsureComponent<ChangeDirectionComponent>(playerEntity, out _);
            command.NewDirection = newDirection;
        }
    }
}