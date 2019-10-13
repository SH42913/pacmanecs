using System;
using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;
using Random = System.Random;

namespace Game.Ghosts.Systems
{
    public class GhostInitSystem : IEcsInitSystem
    {
        private readonly Random _random = null;
        private readonly EcsWorld _ecsWorld = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void Init()
        {
            if (!_gameDefinitions.ghostDefinition)
            {
                throw new Exception($"{nameof(GhostDefinition)} doesn't exists!");
            }

            GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghostObject in ghostObjects)
            {
                EcsEntity ghostEntity = _ecsWorld.NewEntityWith(
                    out GhostComponent ghostComponent,
                    out MoveComponent moveComponent,
                    out GhostInFearStateComponent _);

                switch (ghostObject.name.ToLower())
                {
                    case "pinky":
                        ghostComponent.GhostType = GhostTypes.Pinky;
                        break;
                    case "inky":
                        ghostComponent.GhostType = GhostTypes.Inky;
                        break;
                    case "clyde":
                        ghostComponent.GhostType = GhostTypes.Clyde;
                        break;
                    default:
                        ghostComponent.GhostType = GhostTypes.Blinky;
                        break;
                }

                moveComponent.DesiredPosition = ghostObject.transform.position.ToVector2Int();
                moveComponent.Heading = _random.NextEnum<Directions>();
                moveComponent.Speed = _gameDefinitions.ghostDefinition.GhostSpeed;

                ghostComponent.Renderer = ghostObject.GetComponent<MeshRenderer>();
                ghostEntity.Set<CreateWorldObjectEvent>().Transform = ghostObject.transform;
            }
        }
    }
}