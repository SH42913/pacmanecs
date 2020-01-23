using Game.Moving;
using Game.Ui.GameStates;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Players
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<PlayerComponent> _players = null;

        public void Run()
        {
            foreach (int i in _players)
            {
                int playerNum = _players.Get1[i].Num;
                EcsEntity playerEntity = _players.Entities[i];
                float yAxis = Input.GetAxis($"Player{playerNum.ToString()}Y");
                float xAxis = Input.GetAxis($"Player{playerNum.ToString()}X");

                if (yAxis > 0)
                {
                    SendCommand(Directions.Up, playerEntity);
                }
                else if (yAxis < 0)
                {
                    SendCommand(Directions.Down, playerEntity);
                }
                else if (xAxis > 0)
                {
                    SendCommand(Directions.Right, playerEntity);
                }
                else if (xAxis < 0)
                {
                    SendCommand(Directions.Left, playerEntity);
                }

                if (!Input.GetKeyUp(KeyCode.Escape)) continue;
                _ecsWorld.NewEntityWith(out ChangeGameStateEvent changeGameStateEvent);
                changeGameStateEvent.State = Time.timeScale < 1
                    ? GameStates.Start
                    : GameStates.Pause;
            }
        }

        private void SendCommand(Directions newDirection, EcsEntity playerEntity)
        {
            var command = playerEntity.Set<ChangeDirectionComponent>();
            command.NewDirection = newDirection;
        }
    }
}