namespace Game.UI.GameStates {
    public enum GameStates {
        Start,
        Pause,
        Restart,
        Exit,
        GameOver,
    }

    public struct GameStateSwitchRequest {
        public GameStates newState;
    }
}