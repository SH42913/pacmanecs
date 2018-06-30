using System;
using Components.BaseComponents;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.BaseSystems
{
    [EcsInject]
    public class GameStateSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<GameStateComponent> _states = null;

        public GameObject GuiElement;
        
        public void Run()
        {
            for (int i = 0; i < _states.EntitiesCount; i++)
            {
                switch (_states.Components1[i].State)
                {
                    case GameStates.PAUSE:
                        Time.timeScale = 0f;
                        GuiElement.SetActive(true);
                        break;
                    case GameStates.START:
                        Time.timeScale = 1f;
                        GuiElement.SetActive(false);
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
                
                _ecsWorld.RemoveEntity(_states.Entities[i]);
            }
        }
    }
}