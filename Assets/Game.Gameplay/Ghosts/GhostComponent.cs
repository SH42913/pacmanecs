using UnityEngine;

namespace Game.Gameplay.Ghosts {
    public enum GhostTypes {
        Blinky,
        Pinky,
        Inky,
        Clyde,
    }

    public struct GhostComponent {
        public GhostTypes ghostType;
        public MeshRenderer renderer;
    }
}