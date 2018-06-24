namespace Components
{
    public enum GhostBehaviours
    {
        BLINKY,
        PINKY,
        INKY,
        CLYDE
    }
    
    public class GhostComponent
    {
        public GhostBehaviours Behaviour { get; set; }
    }
}