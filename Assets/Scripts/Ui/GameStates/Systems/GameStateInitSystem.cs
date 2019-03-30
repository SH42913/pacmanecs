using Leopotam.Ecs;
using UnityEngine;

namespace Ui.GameStates.Systems
{
    [EcsInject]
    public class GameStateInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Initialize()
        {
            PauseMenuBehaviour[] menus = Object.FindObjectsOfType<PauseMenuBehaviour>();
            foreach (PauseMenuBehaviour behaviour in menus)
            {
                _ecsWorld.CreateEntityWith(out PauseMenuComponent pauseMenu);
                pauseMenu.GameObject = behaviour.gameObject;
            }

            _ecsWorld.CreateEntityWith(out ChangeGameStateEvent changeGameStateEvent);
            changeGameStateEvent.State = GameStates.START;
        }

        public void Destroy()
        {
        }
    }
}