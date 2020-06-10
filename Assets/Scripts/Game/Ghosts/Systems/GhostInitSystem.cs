using System;
using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;
using Random = System.Random;

namespace Game.Ghosts {
    public class GhostInitSystem : IEcsInitSystem {
        private readonly Random _random = null;
        private readonly EcsWorld _ecsWorld = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void Init() {
            if (!_gameDefinitions.ghostDefinition) {
                throw new Exception($"{nameof(GhostDefinition)} doesn't exists!");
            }

            GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghostObject in ghostObjects) {
                EcsEntity ghostEntity = _ecsWorld.NewEntity();
                ghostEntity
                    .Replace(new GhostInFearStateComponent())
                    .Replace(new GhostComponent {
                        GhostType = GetGhostType(ghostObject.name),
                        Renderer = ghostObject.GetComponent<MeshRenderer>()
                    })
                    .Replace(new MoveComponent {
                        DesiredPosition = ghostObject.transform.position.ToVector2Int(),
                        Heading = _random.NextEnum<Directions>(),
                        Speed = _gameDefinitions.ghostDefinition.GhostSpeed,
                    })
                    .Replace(new CreateWorldObjectEvent {
                        Transform = ghostObject.transform
                    });
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