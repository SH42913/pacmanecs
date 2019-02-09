using UnityEngine;

namespace Ghosts
{
    public class GhostConfigBehaviour : MonoBehaviour
    {
        [Range(0, 10)] 
        public float GhostSpeed;
    }
}