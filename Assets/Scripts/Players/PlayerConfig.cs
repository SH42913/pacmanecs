using UnityEngine;

namespace Players
{
    [CreateAssetMenu(menuName = "PacManEcs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Range(0, 10)]
        public float StartSpeed;
        
        [Range(0, 10)]
        public int StartLives;
    }
}