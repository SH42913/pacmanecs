using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui.GameStates.Systems
{
    [EcsInject]
    public class GameStateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ChangeGameStateEvent> _changeStateEvents = null;
        private readonly EcsFilter<PauseMenuComponent> _menus = null;

        public void Run()
        {
            if (_changeStateEvents.IsEmpty()) return;

            bool needDisableMenu = false;
            bool needEnableMenu = false;
            foreach (int i in _changeStateEvents)
            {
                switch (_changeStateEvents.Components1[i].State)
                {
                    case GameStates.PAUSE:
                        Time.timeScale = 0f;
                        needEnableMenu = true;
                        break;
                    case GameStates.START:
                        Time.timeScale = 1f;
                        needDisableMenu = true;
                        break;
                    case GameStates.RESTART:
                        SceneManager.LoadScene(0);
                        break;
                    case GameStates.EXIT:
                        Application.Quit();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (needDisableMenu == needEnableMenu) return;

            foreach (int i in _menus)
            {
                _menus.Components1[i].GameObject.SetActive(needEnableMenu);
            }
        }
    }
}