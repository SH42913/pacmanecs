using UnityEngine;

namespace Game.World {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(WorldDefinition))]
    public sealed class WorldDefinition : ScriptableObject {
        public int SizeX;
        public int SizeY;
    }
}