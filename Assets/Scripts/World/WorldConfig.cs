using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "PacManEcs/WorldConfig")]
    public class WorldConfig : ScriptableObject
    {
        public int SizeX;
        public int SizeY;
    }
}