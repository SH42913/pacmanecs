﻿using Leopotam.Ecs;
using UnityEngine;

namespace Game.UI.GameStates {
    public sealed class GameStateInputSystem : IEcsInitSystem, IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<PauseMenuComponent> menus = null;

        public void Init() {
            foreach (var i in menus) {
                ref var menu = ref menus.Get1(i);

                menu.continueBtn.onClick.AddListener(ContinueGame);
                menu.restartBtn.onClick.AddListener(RestartGame);
                menu.quitBtn.onClick.AddListener(QuitGame);
            }
        }

        public void Run() {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = Time.timeScale < 1
                    ? GameStates.Start
                    : GameStates.Pause;
            }
        }

        private void ContinueGame() {
            ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Start;
        }

        private void RestartGame() {
            ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Restart;
        }

        private void QuitGame() {
            ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Exit;
        }
    }
}