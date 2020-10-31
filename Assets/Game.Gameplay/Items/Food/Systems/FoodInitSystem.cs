using System;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Items.Food {
    public sealed class FoodInitSystem : IEcsInitSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly GameDefinitions gameDefinitions = null;

        public void Init() {
            if (!gameDefinitions.foodDefinition) throw new Exception($"{nameof(FoodDefinition)} doesn't exists!");

            var foodDefinition = gameDefinitions.foodDefinition;
            var foodObjects = GameObject.FindGameObjectsWithTag("Food");
            foreach (var foodObject in foodObjects) {
                var entity = ecsWorld.NewEntity();
                entity.Replace(new ItemMarker())
                    .Replace(new WorldObjectCreateRequest { transform = foodObject.transform })
                    .Replace(new FoodComponent {
                        scores = foodDefinition.scoresPerFood,
                        speedPenalty = foodDefinition.speedPenalty
                    });
            }

            var energizers = GameObject.FindGameObjectsWithTag("Energizer");
            foreach (var energizer in energizers) {
                var entity = ecsWorld.NewEntity();
                entity.Replace(new EnergizerMarker())
                    .Replace(new ItemMarker())
                    .Replace(new WorldObjectCreateRequest { transform = energizer.transform })
                    .Replace(new FoodComponent {
                        scores = foodDefinition.scoresPerFood * foodDefinition.energizerMultiplier,
                        speedPenalty = foodDefinition.speedPenalty * foodDefinition.energizerMultiplier
                    });
            }
        }
    }
}