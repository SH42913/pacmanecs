using Game.UI.GameStates;
using Game.UI.ScoreTable;
using Leopotam.Ecs;

namespace Game.UI {
    public static class EcsSystemsExtensions {
        public static EcsSystems AddUiSystems(this EcsSystems systems) {
            return systems
                .Add(new GameStateInputSystem())
                .Add(new GameStateSystem())
                .OneFrame<GameStateSwitchRequest>()
                .Add(new ScoreTableSystem());
        }
    }
}