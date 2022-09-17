using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI.GameStates {
    public sealed class GameStateSystem : IEcsRunSystem {
        private readonly EcsFilter<PauseMenuComponent> menus = null;
        private readonly EcsFilter<GameStateSwitchRequest> requests = null;

        public void Run() {
            if (requests.IsEmpty()) return;

            var gameState = GameStates.Start;
            var needDisableMenu = false;
            var needEnableMenu = false;
            foreach (var i in requests) {
                gameState = requests.Get1(i).newState;
                switch (gameState) {
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
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                        break;
                    case GameStates.GameOver:
                        needEnableMenu = true;
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            if (needDisableMenu == needEnableMenu) return;

            foreach (var i in menus) {
                ref var menu = ref menus.Get1(i);

                var isGameOver = gameState == GameStates.GameOver;
                menu.menuText.gameObject.SetActive(isGameOver);
                menu.continueBtn.interactable = !isGameOver;
                menu.root.SetActive(needEnableMenu);
            }
        }
    }
}