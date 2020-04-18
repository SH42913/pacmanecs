using UnityEngine;

namespace Game.Items.Food {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(FoodDefinition))]
    public sealed class FoodDefinition : ScriptableObject {
        public int ScoresPerFood;

        [Range(0, 0.01f)] public float SpeedPenalty;

        [Range(1, 10)] public int EnergizerMultiplier;
    }
}