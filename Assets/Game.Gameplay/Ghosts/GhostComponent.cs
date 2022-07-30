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
        public Renderer renderer;
        public MaterialPropertyBlock materialPropertyBlock;
    }

    public static class GhostComponentExtensions {
        private static readonly int mainColorId = Shader.PropertyToID("_Color");

        public static void SetColor(ref this GhostComponent ghost, Color color) {
            ghost.materialPropertyBlock.SetColor(mainColorId, color);
            ghost.renderer.SetPropertyBlock(ghost.materialPropertyBlock);
        }
    }
}