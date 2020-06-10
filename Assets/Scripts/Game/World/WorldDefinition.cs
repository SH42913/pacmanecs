using UnityEngine;
using UnityEngine.Serialization;

namespace Game.World {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(WorldDefinition))]
    public sealed class WorldDefinition : ScriptableObject {
        public int sizeX;
        public int sizeY;
    }
}