using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Ui.GameStates {
    public class GameStateSystem : IEcsRunSystem {
        private readonly EcsFilter<PauseMenuComponent> _menus = null;
        private readonly EcsFilter<ChangeGameStateEvent> _changeStateEvents = null;

        public void Run() {
            if (_changeStateEvents.IsEmpty()) return;

            bool needDisableMenu = false;
            bool needEnableMenu = false;
            foreach (int i in _changeStateEvents) {
                switch (_changeStateEvents.Get1(i).State) {
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (needDisableMenu == needEnableMenu) return;

            foreach (int i in _menus) {
                _menus.Get1(i).GameObject.SetActive(needEnableMenu);
            }
        }
    }
}