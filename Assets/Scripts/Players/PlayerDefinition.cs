using UnityEngine;

namespace Players
{
    [CreateAssetMenu(menuName = "PacManEcs/PlayerDefinition", fileName = "PlayerDef")]
    public class PlayerDefinition : ScriptableObject
    {
        [Range(0, 10)] public float StartSpeed;

        [Range(0, 10)] public int StartLives;
    }
}