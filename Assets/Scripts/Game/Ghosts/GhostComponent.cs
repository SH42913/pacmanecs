using UnityEngine;

namespace Game.Ghosts {
    public enum GhostTypes {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }

    public struct GhostComponent {
        public GhostTypes GhostType;
        public MeshRenderer Renderer;
    }
}