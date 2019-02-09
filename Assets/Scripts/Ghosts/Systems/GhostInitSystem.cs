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

        private static readonly Random Random = new Random();

        public void Initialize()
        {
            var ghostConfig = Object.FindObjectOfType<GhostConfigBehaviour>();
            if (ghostConfig == null)
            {
                throw new Exception("GhostConfigBehaviour must be created!");
            }

            GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghostObject in ghostObjects)
            {
                switch (ghostObject.name.ToLower())
                {
                    case "blinky":
                        break;
                    case "pinky":
                        break;
                    case "inky":
                        break;
                    case "clyde":
                        break;
                }

                MoveComponent moveComponent;
                GhostComponent ghostComponent;
                var ghostEntity = _ecsWorld.CreateEntityWith(out ghostComponent, out moveComponent);

                moveComponent.DesiredPosition = ghostObject.transform.position.ToVector2Int();
                moveComponent.Heading = Random.NextEnum<Directions>();
                moveComponent.Speed = ghostConfig.GhostSpeed;

                _ecsWorld.AddComponent<CreateWorldObjectEvent>(ghostEntity).Transform = ghostObject.transform;
            }
        }

        public void Destroy()
        {
        }
    }
}