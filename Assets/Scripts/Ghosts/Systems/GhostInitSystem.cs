using System;
using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;
using Random = System.Random;

namespace Ghosts.Systems
{
    [EcsInject]
    public class GhostInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly Random _random = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void Initialize()
        {
            if (!_mainGameConfig.GhostConfig)
            {
                throw new Exception($"{nameof(GhostConfig)} doesn't exists!");
            }
            
            GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghostObject in ghostObjects)
            {
                EcsEntity ghostEntity = _ecsWorld.CreateEntityWith(
                    out GhostComponent ghostComponent, 
                    out MoveComponent moveComponent, 
                    out GhostInFearStateComponent _);

                switch (ghostObject.name.ToLower())
                {
                    case "pinky":
                        ghostComponent.GhostType = GhostTypes.PINKY;
                        break;
                    case "inky":
                        ghostComponent.GhostType = GhostTypes.INKY;
                        break;
                    case "clyde":
                        ghostComponent.GhostType = GhostTypes.CLYDE;
                        break;
                    default:
                        ghostComponent.GhostType = GhostTypes.BLINKY;
                        break;
                }

                moveComponent.DesiredPosition = ghostObject.transform.position.ToVector2Int();
                moveComponent.Heading = _random.NextEnum<Directions>();
                moveComponent.Speed = _mainGameConfig.GhostConfig.GhostSpeed;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(ghostEntity).Transform = ghostObject.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}