using System;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Items.Food {
    public sealed class FoodInitSystem : IEcsInitSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly GameDefinitions gameDefinitions = null;

        private readonly Transform[] foodTransforms;
        private readonly Transform[] energizerTransforms;

        public FoodInitSystem(Transform[] foodTransforms, Transform[] energizerTransforms) {
            this.foodTransforms = foodTransforms;
            this.energizerTransforms = energizerTransforms;
        }

        public void Init() {
            if (!gameDefinitions.foodDefinition) throw new Exception($"{nameof(FoodDefinition)} doesn't exists!");

            var definition = gameDefinitions.foodDefinition;
            foreach (var foodObject in foodTransforms) {
                var entity = ecsWorld.NewEntity();
                entity.Replace(new ItemMarker())
                    .Replace(new WorldObjectCreateRequest { transform = foodObject.transform })
                    .Replace(new FoodComponent {
                        scores = definition.scoresPerFood,
                        speedPenalty = definition.speedPenalty,
                    });
            }

            foreach (var energizer in energizerTransforms) {
                var entity = ecsWorld.NewEntity();
                entity.Replace(new EnergizerMarker())
                    .Replace(new ItemMarker())
                    .Replace(new WorldObjectCreateRequest { transform = energizer.transform })
                    .Replace(new FoodComponent {
                        scores = definition.scoresPerFood * definition.energizerMultiplier,
                        speedPenalty = definition.speedPenalty * definition.energizerMultiplier,
                    });
            }
        }
    }
}