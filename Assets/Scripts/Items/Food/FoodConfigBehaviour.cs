using UnityEngine;

namespace Items.Food
{
    public class FoodConfigBehaviour : MonoBehaviour
    {
        public int ScoresPerFood;
        
        [Range(0, 0.01f)]
        public float SpeedPenalty;
        
        [Range(1, 10)]
        public int EnergizerMultiplier;
    }
}