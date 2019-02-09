using System;
using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Ghosts.Systems
{
    [EcsInject]
    public class GhostInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<WorldComponent> _world = null;

        private static readonly Random Random = new Random();

        public void Initialize()
        {
            var ghostConfigBehaviour = Object.FindObjectOfType<GhostConfigBehaviour>();
            if (ghostConfigBehaviour == null)
            {
                throw new Exception("GhostConfigBehaviour must be created!");
            }

            int worldEntity = _world.Entities[0];
            var ghostConfig = _ecsWorld.AddComponent<GhostConfigComponent>(worldEntity);
            ghostConfig.FearStateInSec = ghostConfigBehaviour.FearStateInSec;
            ghostConfig.ScoresPerGhost = ghostConfigBehaviour.ScoresPerGhost;
            ghostConfig.Blinky = ghostConfigBehaviour.Blinky;
            ghostConfig.Pinky = ghostConfigBehaviour.Pinky;
            ghostConfig.Inky = ghostConfigBehaviour.Inky;
            ghostConfig.Clyde = ghostConfigBehaviour.Clyde;
            ghostConfig.FearState = ghostConfigBehaviour.FearState;

            GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghostObject in ghostObjects)
            {
                MoveComponent moveComponent;
                GhostComponent ghostComponent;
                GhostInFearStateComponent fearState;
                var ghostEntity = _ecsWorld.CreateEntityWith(out ghostComponent, out moveComponent, out fearState);

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
                moveComponent.Heading = Random.NextEnum<Directions>();
                moveComponent.Speed = ghostConfigBehaviour.GhostSpeed;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(ghostEntity).Transform = ghostObject.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}