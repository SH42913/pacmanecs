namespace Components.BaseComponents
{
    public enum GameStates
    {
        START,
        PAUSE,
        RESTART,
        EXIT
    }
    
    public class GameStateComponent
    {
        public GameStates State;
    }
}