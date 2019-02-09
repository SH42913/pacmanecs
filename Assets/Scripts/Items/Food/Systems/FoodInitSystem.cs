using System;
using Leopotam.Ecs;
using UnityEngine;
using World;
using Object = UnityEngine.Object;

namespace Items.Food.Systems
{
    [EcsInject]
    public class FoodInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Initialize()
        {
            var foodConfig = Object.FindObjectOfType<FoodConfigBehaviour>();
            if (foodConfig == null)
            {
                throw new Exception("FoodConfigBehaviour must be created!");
            }

            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject foodObject in foodObjects)
            {
                ItemComponent itemComponent;
                FoodComponent foodComponent;
                int entity = _ecsWorld.CreateEntityWith(out itemComponent, out foodComponent);

                foodComponent.Scores = foodConfig.ScoresPerFood;
                foodComponent.SpeedPenalty = foodConfig.SpeedPenalty;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(entity).Transform = foodObject.transform;
            }

            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");
            foreach (GameObject energizer in energizers)
            {
                ItemComponent itemComponent;
                FoodComponent foodComponent;
                EnergizerComponent energizerComponent;
                int entity = _ecsWorld.CreateEntityWith(out itemComponent, out foodComponent, out energizerComponent);

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