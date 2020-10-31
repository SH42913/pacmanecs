using UnityEngine;

namespace Game.Gameplay.Items.Food {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(FoodDefinition))]
    public sealed class FoodDefinition : ScriptableObject {
        public int scoresPerFood;
        [Range(0, 0.01f)] public float speedPenalty;
        [Range(1, 10)] public int energizerMultiplier;
    }
}