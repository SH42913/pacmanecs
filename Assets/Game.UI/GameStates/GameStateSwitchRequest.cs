namespace Game.UI.GameStates {
    public enum GameStates {
        Start,
        Pause,
        Restart,
        Exit,
    }

    public struct GameStateSwitchRequest {
        public GameStates newState;
    }
}