using Components.BaseComponents;
using Components.GhostComponents;
using Components.PlayerComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.GhostSystems
{
    [EcsInject]
    public class GhostSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float GhostSpeed = 2f;

        private EcsWorld _ecsWorld = null;
        private EcsFilter<PositionComponent, MoveComponent, GhostComponent> _ghosts = null;
        private EcsFilter<PositionComponent, PlayerComponent> _players = null;
        
        public void Initialize()
        {
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
                    default:
                        break;
                }

                int entity = ghostObject.CreateEntityWithPosition(_ecsWorld);
                _ecsWorld.AddComponent<GhostComponent>(entity);
                
                var moveComponent = _ecsWorld.AddComponent<MoveComponent>(entity);
                moveComponent.DesiredPosition = ghostObject.transform.position;
                moveComponent.Heading = Directions.LEFT;
                moveComponent.Speed = GhostSpeed;
                moveComponent.Transform = ghostObject.transform;

            }
        }

        public void Run()
        {
            for (int ghostIndex = 0; ghostIndex < _ghosts.EntitiesCount; ghostIndex++)
            {
                var currentPosition = _ghosts.Components1[ghostIndex].Position;
                var moveComponent = _ghosts.Components2[ghostIndex];
                var targetPosition = moveComponent.DesiredPosition.ToVector2Int();

                for (int playerIndex = 0; playerIndex < _players.EntitiesCount; playerIndex++)
                {
                    if(!_players.Components1[playerIndex].Position.Equals(currentPosition)) continue;
                    
                    _ecsWorld
                        .CreateEntityWith<DeathComponent>()
                        .PlayerEntity = _players.Entities[playerIndex];
                    break;
                }

                if(currentPosition != targetPosition) continue;
                moveComponent.Heading = (Directions) Random.Range(0, 4);
            }
        }

        public void Destroy()
        {}
    }
}