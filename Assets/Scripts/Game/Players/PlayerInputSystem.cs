﻿using Game.Moving;
using Game.Ui.GameStates;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Players {
    public sealed class PlayerInputSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<PlayerComponent, MovementComponent> players = null;

        public void Run() {
            foreach (var i in players) {
                var playerNum = players.Get1(i).num;
                var yAxis = Input.GetAxis($"Player{playerNum.ToString()}Y");
                var xAxis = Input.GetAxis($"Player{playerNum.ToString()}X");

                ref var movement = ref players.Get2(i);
                if (yAxis > 0) {
                    movement.heading = Directions.Up;
                } else if (yAxis < 0) {
                    movement.heading = Directions.Down;
                } else if (xAxis > 0) {
                    movement.heading = Directions.Right;
                } else if (xAxis < 0) {
                    movement.heading = Directions.Left;
                }

                if (Input.GetKeyUp(KeyCode.Escape)) {
                    ecsWorld.NewEntity().Get<ChangeGameStateEvent>().state = Time.timeScale < 1
                        ? GameStates.Start
                        : GameStates.Pause;
                }
            }
        }
    }
}