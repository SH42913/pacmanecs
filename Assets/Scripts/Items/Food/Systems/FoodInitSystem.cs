using System;
using Leopotam.Ecs;
using UnityEngine;
using World;

namespace Items.Food.Systems
{
    [EcsInject]
    public class FoodInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void Initialize()
        {
            if (!_mainGameConfig.FoodConfig)
            {
                throw new Exception($"{nameof(FoodConfig)} doesn't exists!");
            }
            
            FoodConfig foodConfig = _mainGameConfig.FoodConfig;
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject foodObject in foodObjects)
            {
                int entity = _ecsWorld.CreateEntityWith(out FoodComponent foodComponent, out ItemComponent _);

                foodComponent.Scores = foodConfig.ScoresPerFood;
                foodComponent.SpeedPenalty = foodConfig.SpeedPenalty;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(entity).Transform = foodObject.transform;
            }

            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");
            foreach (GameObject energizer in energizers)
            {
                int entity = _ecsWorld.CreateEntityWith(
                    out FoodComponent foodComponent, 
                    out ItemComponent _, out EnergizerComponent _);

                foodComponent.Scores = foodConfig.ScoresPerFood * foodConfig.EnergizerMultiplier;
                foodComponent.SpeedPenalty = foodConfig.SpeedPenalty * foodConfig.EnergizerMultiplier;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(entity).Transform = energizer.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}