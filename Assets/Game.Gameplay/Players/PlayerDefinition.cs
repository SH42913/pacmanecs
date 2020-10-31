using UnityEngine;

namespace Game.Gameplay.Players {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(PlayerDefinition))]
    public sealed class PlayerDefinition : ScriptableObject {
        [Range(0, 10)] public float startSpeed;
        [Range(0, 10)] public int startLives;
    }
}