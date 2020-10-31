using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Ui.GameStates {
    public sealed class GameStateSystem : IEcsRunSystem {
        private readonly EcsFilter<PauseMenuComponent> menus = null;
        private readonly EcsFilter<GameStateSwitchRequest> requests = null;

        public void Run() {
            if (requests.IsEmpty()) return;

            var needDisableMenu = false;
            var needEnableMenu = false;
            foreach (var i in requests) {
                switch (requests.Get1(i).state) {
                    case GameStates.Pause:
                        Time.timeScale = 0f;
                        needEnableMenu = true;
                        break;
                    case GameStates.Start:
                        Time.timeScale = 1f;
                        needDisableMenu = true;
                        break;
                    case GameStates.Restart:
                        SceneManager.LoadScene(0);
                        break;
                    case GameStates.Exit:
                        Application.Quit();
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            if (needDisableMenu == needEnableMenu) return;

            foreach (var i in menus) {
                menus.Get1(i).gameObject.SetActive(needEnableMenu);
            }
        }
    }
}