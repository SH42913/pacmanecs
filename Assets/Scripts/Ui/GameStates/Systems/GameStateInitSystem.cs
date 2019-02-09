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
            var menus = Object.FindObjectsOfType<PauseMenuBehaviour>();
            foreach (PauseMenuBehaviour behaviour in menus)
            {
                _ecsWorld.CreateEntityWith<PauseMenuComponent>().GameObject = behaviour.gameObject;
            }

            _ecsWorld.CreateEntityWith<ChangeGameStateEvent>().State = GameStates.START;
        }

        public void Destroy()
        {
        }
    }
}