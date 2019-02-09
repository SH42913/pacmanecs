using Leopotam.Ecs;

namespace Ui.GameStates
{
    public enum GameStates
    {
        START,
        PAUSE,
        RESTART,
        EXIT
    }

    [EcsOneFrame]
    public class ChangeGameStateEvent
    {
        public GameStates State;
    }
}