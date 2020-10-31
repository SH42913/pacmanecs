using UnityEngine;

namespace Game.Gameplay.World {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(WorldDefinition))]
    public sealed class WorldDefinition : ScriptableObject {
        public int sizeX;
        public int sizeY;
    }
}