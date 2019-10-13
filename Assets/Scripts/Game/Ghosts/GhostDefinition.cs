using UnityEngine;

namespace Game.Ghosts
{
    [CreateAssetMenu(menuName = "PacManEcs/GhostDefinition", fileName = "GhostDef")]
    public class GhostDefinition : ScriptableObject
    {
        [Range(0, 10)] public float GhostSpeed;

        [Range(0, 10)] public float FearStateInSec;
        public int ScoresPerGhost;

        public Color Blinky;
        public Color Pinky;
        public Color Inky;
        public Color Clyde;
        public Color FearState;
    }
}