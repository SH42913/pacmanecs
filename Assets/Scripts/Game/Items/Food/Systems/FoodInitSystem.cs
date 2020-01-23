using System;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Items.Food
{
    public class FoodInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void Init()
        {
            if (!_gameDefinitions.foodDefinition)
            {
                throw new Exception($"{nameof(FoodDefinition)} doesn't exists!");
            }

            FoodDefinition foodDefinition = _gameDefinitions.foodDefinition;
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject foodObject in foodObjects)
            {
                EcsEntity entity = _ecsWorld.NewEntityWith(out FoodComponent foodComponent, out ItemComponent _);

                foodComponent.Scores = foodDefinition.ScoresPerFood;
                foodComponent.SpeedPenalty = foodDefinition.SpeedPenalty;

                entity.Set<CreateWorldObjectEvent>().Transform = foodObject.transform;
            }

            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");
            foreach (GameObject energizer in energizers)
            {
                EcsEntity entity = _ecsWorld.NewEntityWith(
                    out FoodComponent foodComponent,
                    out EnergizerComponent _,
                    out ItemComponent _);

                foodComponent.Scores = foodDefinition.ScoresPerFood * foodDefinition.EnergizerMultiplier;
                foodComponent.SpeedPenalty = foodDefinition.SpeedPenalty * foodDefinition.EnergizerMultiplier;

                entity.Set<CreateWorldObjectEvent>().Transform = energizer.transform;
            }
        }
    }
}