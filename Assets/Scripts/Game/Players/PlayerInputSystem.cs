using Game.Moving;
using Game.Ui.GameStates;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Players {
    public sealed class PlayerInputSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<PlayerComponent> players = null;

        public void Run() {
            foreach (var i in players) {
                var playerNum = players.Get1(i).num;
                var playerEntity = players.GetEntity(i);
                var yAxis = Input.GetAxis($"Player{playerNum.ToString()}Y");
                var xAxis = Input.GetAxis($"Player{playerNum.ToString()}X");

                if (yAxis > 0) {
                    SendCommand(Directions.Up, playerEntity);
                } else if (yAxis < 0) {
                    SendCommand(Directions.Down, playerEntity);
                } else if (xAxis > 0) {
                    SendCommand(Directions.Right, playerEntity);
                } else if (xAxis < 0) {
                    SendCommand(Directions.Left, playerEntity);
                }

                if (Input.GetKeyUp(KeyCode.Escape)) {
                    ecsWorld.NewEntity().Get<ChangeGameStateEvent>().state = Time.timeScale < 1
                        ? GameStates.Start
                        : GameStates.Pause;
                }
            }
        }

        private static void SendCommand(Directions newDirection, EcsEntity playerEntity) {
            playerEntity.Get<ChangeDirectionEvent>().newDirection = newDirection;
        }
    }
}