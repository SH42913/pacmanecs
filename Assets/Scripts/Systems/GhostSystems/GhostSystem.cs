using Components.BaseComponents;
using Components.GhostComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;
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
            for (int i = 0; i < _ghosts.EntitiesCount; i++)
            {
                var currentPosition = _ghosts.Components1[i].Position;
                var moveComponent = _ghosts.Components2[i];
                var targetPosition = moveComponent.DesiredPosition.ToVector2Int();
                
                var deadPlayer = _players.GetSecondComponent(x => x.Position == currentPosition);
                if (deadPlayer != null)
                {
                    _ecsWorld
                        .CreateEntityWith<DeathComponent>()
                        .Player = deadPlayer;
                }

                if(currentPosition != targetPosition) continue;
                moveComponent.Heading = (Directions) Random.Range(0, 4);
            }
        }

        public void Destroy()
        {}

        private Vector2Int GetBlinkyTargetCell()
        {
            return Vector2Int.zero;
        }
    }
}