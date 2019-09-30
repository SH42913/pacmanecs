using Leopotam.Ecs;

namespace Ui.GameStates
{
    public enum GameStates
    {
        Start,
        Pause,
        Restart,
        Exit
    }

    public class ChangeGameStateEvent : IEcsOneFrame
    {
        public GameStates State;
    }
}