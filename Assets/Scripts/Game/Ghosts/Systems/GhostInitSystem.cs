using System;
using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;
using Random = System.Random;

namespace Game.Ghosts {
    public sealed class GhostInitSystem : IEcsInitSystem {
        private readonly Random random = null;
        private readonly EcsWorld ecsWorld = null;
        private readonly GameDefinitions gameDefinitions = null;

        public void Init() {
            if (!gameDefinitions.ghostDefinition) throw new Exception($"{nameof(GhostDefinition)} doesn't exists!");

            var ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (var ghostObject in ghostObjects) {
                var ghostEntity = ecsWorld.NewEntity();
                ghostEntity.Replace(new GhostInFearStateComponent())
                    .Replace(new GhostComponent {
                        ghostType = GetGhostType(ghostObject.name),
                        renderer = ghostObject.GetComponent<MeshRenderer>()
                    })
                    .Replace(new MoveComponent {
                        desiredPosition = ghostObject.transform.position.ToVector2Int(),
                        heading = random.NextEnum<Directions>(),
                        speed = gameDefinitions.ghostDefinition.ghostSpeed,
                    })
                    .Replace(new CreateWorldObjectEvent { transform = ghostObject.transform });
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