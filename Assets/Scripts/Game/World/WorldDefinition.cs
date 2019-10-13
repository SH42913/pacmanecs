using UnityEngine;

namespace Game.World
{
    [CreateAssetMenu(menuName = "PacManEcs/WorldDefinition", fileName = "WorldDef")]
    public class WorldDefinition : ScriptableObject
    {
        public int SizeX;
        public int SizeY;
    }
}