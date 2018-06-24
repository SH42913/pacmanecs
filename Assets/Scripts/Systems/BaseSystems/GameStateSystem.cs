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
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<GameStateComponent> States { get; set; }
        
        public GameObject GuiElement { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < States.EntitiesCount; i++)
            {
                switch (States.Components1[i].State)
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
                
                EcsWorld.RemoveEntity(States.Entities[i]);
            }
        }
    }
}