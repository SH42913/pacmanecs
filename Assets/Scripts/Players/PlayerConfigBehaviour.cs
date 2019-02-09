using UnityEngine;

namespace Players
{
    public class PlayerConfigBehaviour : MonoBehaviour
    {
        [Range(0, 10)]
        public float StartSpeed;
        
        [Range(0, 10)]
        public int StartLives;
    }
}