namespace Game.Ui.GameStates
{
    public enum GameStates
    {
        Start,
        Pause,
        Restart,
        Exit
    }

    public class ChangeGameStateEvent
    {
        public GameStates State;
    }
}