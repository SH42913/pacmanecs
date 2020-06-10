using System;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Items.Food {
    public class FoodInitSystem : IEcsInitSystem {
        private readonly EcsWorld _ecsWorld = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void Init() {
            if (!_gameDefinitions.foodDefinition) {
                throw new Exception($"{nameof(FoodDefinition)} doesn't exists!");
            }

            FoodDefinition foodDefinition = _gameDefinitions.foodDefinition;
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject foodObject in foodObjects) {
                EcsEntity entity = _ecsWorld.NewEntity();
                entity.Replace(new ItemComponent())
                    .Replace(new FoodComponent {
                        Scores = foodDefinition.ScoresPerFood,
                        SpeedPenalty = foodDefinition.SpeedPenalty
                    })
                    .Replace(new CreateWorldObjectEvent {
                        Transform = foodObject.transform
                    });
            }

            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");
            foreach (GameObject energizer in energizers) {
                EcsEntity entity = _ecsWorld.NewEntity();
                entity.Replace(new EnergizerComponent())
                    .Replace(new ItemComponent())
                    .Replace(new FoodComponent {
                        Scores = foodDefinition.ScoresPerFood * foodDefinition.EnergizerMultiplier,
                        SpeedPenalty = foodDefinition.SpeedPenalty * foodDefinition.EnergizerMultiplier
                    })
                    .Replace(new CreateWorldObjectEvent {
                        Transform = energizer.transform
                    });
            }
        }
    }
}