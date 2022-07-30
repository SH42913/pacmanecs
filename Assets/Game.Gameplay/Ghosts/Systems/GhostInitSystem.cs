using System;
using Game.Gameplay.Movement;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Ghosts {
    public sealed class GhostInitSystem : IEcsInitSystem {
        private readonly System.Random random = null;
        private readonly EcsWorld ecsWorld = null;
        private readonly GameDefinitions gameDefinitions = null;

        public void Init() {
            if (!gameDefinitions.ghostDefinition) throw new Exception($"{nameof(GhostDefinition)} doesn't exists!");

            var ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (var ghostObject in ghostObjects) {
                var renderer = ghostObject.GetComponent<Renderer>();
                var materialPropertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(materialPropertyBlock);

                var ghostEntity = ecsWorld.NewEntity();
                ghostEntity.Replace(new GhostInFearStateComponent())
                    .Replace(new GhostComponent {
                        ghostType = GetGhostType(ghostObject.name),
                        renderer = renderer,
                        materialPropertyBlock = materialPropertyBlock,
                    })
                    .Replace(new MovementComponent {
                        desiredPosition = ghostObject.transform.position.ToVector2Int(),
                        heading = random.NextEnum<Directions>(),
                        speed = gameDefinitions.ghostDefinition.ghostSpeed,
                    })
                    .Replace(new WorldObjectCreateRequest { transform = ghostObject.transform });
            }
        }

        private static GhostTypes GetGhostType(string name) {
            switch (name.ToLower()) {
                case "pinky": return GhostTypes.Pinky;
                case "inky": return GhostTypes.Inky;
                case "clyde": return GhostTypes.Clyde;
                default: return GhostTypes.Blinky;
            }
        }
    }
}