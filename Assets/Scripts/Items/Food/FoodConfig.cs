using UnityEngine;

namespace Items.Food
{
    [CreateAssetMenu(menuName = "PacManEcs/FoodConfig")]
    public class FoodConfig : ScriptableObject
    {
        public int ScoresPerFood;
        
        [Range(0, 0.01f)]
        public float SpeedPenalty;
        
        [Range(1, 10)]
        public int EnergizerMultiplier;
    }
}