using Leopotam.Ecs;
using UnityEngine;

namespace Game.UI.GameStates {
    public sealed class GameStateInputSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;

        public void Run() {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = Time.timeScale < 1
                    ? GameStates.Start
                    : GameStates.Pause;
            }
        }
    }
}