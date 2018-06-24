using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class DeathSystem : IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<DeathComponent> DeathComponents { get; set; }
        private EcsFilter<PositionComponent, MoveComponent, PlayerComponent> Players { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < DeathComponents.EntitiesCount; i++)
            {
                var targetPlayer = DeathComponents.Components1[i].Player;
                PositionComponent deadPositionComponent = null;
                MoveComponent deadMoveComponent = null;

                for (int j = 0; j < Players.EntitiesCount; j++)
                {
                    if(!Players.Components3[i].Equals(targetPlayer)) continue;
                    deadPositionComponent = Players.Components1[i];
                    deadMoveComponent = Players.Components2[i];
                    break;
                }
                
                targetPlayer.Lifes--;
                Vector3 respawnVector = targetPlayer.Lifes > 0
                    ? targetPlayer.StartPosition
                    : Vector3.zero;

                if (deadPositionComponent != null)
                {
                    deadPositionComponent.Position = respawnVector.ToVector2Int();
                }

                if (deadMoveComponent != null)
                {
                    deadMoveComponent.Transform.position = respawnVector;
                    deadMoveComponent.DesiredPosition = respawnVector;
                }
                
                if (targetPlayer.Lifes == 0)
                {
                    targetPlayer.IsDead = true;
                    deadMoveComponent?.Transform?.gameObject.SetActive(false);
                }
                
                EcsWorld.CreateEntityWith<UpdateGuiComponent>();
                EcsWorld.RemoveEntity(DeathComponents.Entities[i]);
            }
        }
    }
}