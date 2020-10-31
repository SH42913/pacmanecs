using UnityEngine;

namespace Game.Gameplay.Ghosts {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(GhostDefinition))]
    public sealed class GhostDefinition : ScriptableObject {
        [Range(0, 10)] public float ghostSpeed;
        [Range(0, 10)] public float fearStateInSec;

        public int scoresPerGhost;

        public Color blinky;
        public Color pinky;
        public Color inky;
        public Color clyde;
        public Color fearState;
    }
}