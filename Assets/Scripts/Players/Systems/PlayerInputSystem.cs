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
                EcsEntity playerEntity = _players.Entities[i];
                float yAxis = Input.GetAxis($"Player{playerNum}Y");
                float xAxis = Input.GetAxis($"Player{playerNum}X");

                if (yAxis > 0)
                {
                    SendCommand(Directions.UP, playerEntity);
                }
                else if (yAxis < 0)
                {
                    SendCommand(Directions.DOWN, playerEntity);
                }
                else if (xAxis > 0)
                {
                    SendCommand(Directions.RIGHT, playerEntity);
                }
                else if (xAxis < 0)
                {
                    SendCommand(Directions.LEFT, playerEntity);
                }

                if (!Input.GetKeyUp(KeyCode.Escape)) continue;
                _ecsWorld.CreateEntityWith(out ChangeGameStateEvent changeGameStateEvent);
                changeGameStateEvent.State = Time.timeScale < 1
                    ? GameStates.START
                    : GameStates.PAUSE;
            }
        }

        private void SendCommand(Directions newDirection, EcsEntity playerEntity)
        {
            var command = _ecsWorld.EnsureComponent<ChangeDirectionComponent>(playerEntity, out _);
            command.NewDirection = newDirection;
        }
    }
}