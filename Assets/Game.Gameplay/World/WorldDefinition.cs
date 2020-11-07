using UnityEngine;

namespace Game.Gameplay.World {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(WorldDefinition))]
    public sealed class WorldDefinition : ScriptableObject {
        public int sizeX;
        public int sizeY;

        public Vector2Int worldSize => new Vector2Int(sizeX, sizeY);
    }
}