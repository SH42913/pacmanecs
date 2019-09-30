using System;
using Leopotam.Ecs;
using UnityEngine;
using World;

namespace Items.Food.Systems
{
    public class FoodInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void Init()
        {
            if (!_mainGameConfig.FoodConfig)
            {
                throw new Exception($"{nameof(FoodConfig)} doesn't exists!");
            }

            FoodConfig foodConfig = _mainGameConfig.FoodConfig;
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject foodObject in foodObjects)
            {
                EcsEntity entity = _ecsWorld.NewEntityWith(out FoodComponent foodComponent, out ItemComponent _);

                foodComponent.Scores = foodConfig.ScoresPerFood;
                foodComponent.SpeedPenalty = foodConfig.SpeedPenalty;

                entity.Set<CreateWorldObjectEvent>().Transform = foodObject.transform;
            }

            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");
            foreach (GameObject energizer in energizers)
            {
                EcsEntity entity = _ecsWorld.NewEntityWith(
                    out FoodComponent foodComponent,
                    out EnergizerComponent _,
                    out ItemComponent _);

                foodComponent.Scores = foodConfig.ScoresPerFood * foodConfig.EnergizerMultiplier;
                foodComponent.SpeedPenalty = foodConfig.SpeedPenalty * foodConfig.EnergizerMultiplier;

                entity.Set<CreateWorldObjectEvent>().Transform = energizer.transform;
            }
        }
    }
}