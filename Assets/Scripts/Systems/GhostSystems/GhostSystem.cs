using Components;
using Components.BaseComponents;
using Components.GhostComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class GhostSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float GhostSpeed { get; set; } = 2f;
        
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<PositionComponent,MoveComponent, GhostComponent> Ghosts { get; set; }
        private EcsFilter<PositionComponent, PlayerComponent> Players { get; set; }
        
        public void Initialize()
        {
            GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");

            foreach (GameObject ghostObject in ghostObjects)
            {
                GhostBehaviours behaviour;
                switch (ghostObject.name.ToLower())
                {
                    case "blinky":
                        behaviour = GhostBehaviours.BLINKY;
                        break;
                    case "pinky":
                        behaviour = GhostBehaviours.PINKY;
                        break;
                    case "inky":
                        behaviour = GhostBehaviours.INKY;
                        break;
                    case "clyde":
                        behaviour = GhostBehaviours.CLYDE;
                        break;
                    default:
                        behaviour = GhostBehaviours.BLINKY;
                        break;
                }

                int entity = ghostObject.CreateEntityWithPosition(EcsWorld);
                var moveComponent = EcsWorld.AddComponent<MoveComponent>(entity);
                moveComponent.DesiredPosition = ghostObject.transform.position;
                moveComponent.Heading = Directions.LEFT;
                moveComponent.Speed = GhostSpeed;
                moveComponent.Transform = ghostObject.transform;

                EcsWorld
                    .AddComponent<GhostComponent>(entity)
                    .Behaviour = behaviour;
            }
        }

        public void Run()
        {
            for (int i = 0; i < Ghosts.EntitiesCount; i++)
            {
                var currentPosition = Ghosts.Components1[i].Position;
                var moveComponent = Ghosts.Components2[i];
                var targetPosition = moveComponent.DesiredPosition.ToVector2Int();
                
                var deadPlayer = Players.GetSecondComponent(x => x.Position == currentPosition);
                if (deadPlayer != null)
                {
                    EcsWorld
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