using System;
using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;
using Random = System.Random;

namespace Ghosts.Systems
{
    public class GhostInitSystem : IEcsInitSystem
    {
        private readonly Random _random = null;
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void Init()
        {
            if (!_mainGameConfig.GhostConfig)
            {
                throw new Exception($"{nameof(GhostConfig)} doesn't exists!");
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
                moveComponent.Speed = _mainGameConfig.GhostConfig.GhostSpeed;

                ghostComponent.Renderer = ghostObject.GetComponent<MeshRenderer>();
                ghostEntity.Set<CreateWorldObjectEvent>().Transform = ghostObject.transform;
            }
        }
    }
}