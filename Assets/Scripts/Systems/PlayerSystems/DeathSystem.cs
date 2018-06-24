using Components.BaseComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class DeathSystem : IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<DeathComponent> DeathComponents { get; set; }
        private EcsFilter<MoveComponent, PlayerComponent> Players { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < DeathComponents.EntitiesCount; i++)
            {
                var targetPlayer = DeathComponents.Components1[i].Player;
                MoveComponent deadMoveComponent = Players.GetFirstComponent(null, x => x.Equals(targetPlayer));
                
                if (--targetPlayer.Lifes <= 0)
                {
                    targetPlayer.IsDead = true;
                    deadMoveComponent?.Transform.gameObject.SetActive(false);
                }
                
                Vector3 respawnVector = targetPlayer.Lifes > 0
                    ? targetPlayer.StartPosition
                    : Vector3.zero;

                var teleportComponent = EcsWorld.CreateEntityWith<TeleportComponent>();
                teleportComponent.MoveComponent = deadMoveComponent;
                teleportComponent.TargetPosition = respawnVector;
                
                EcsWorld.CreateEntityWith<UpdateGuiComponent>();
                RemoveEntitiesWith(targetPlayer);
            }
        }

        private void RemoveEntitiesWith(PlayerComponent playerComponent)
        {
            for (int i = 0; i < DeathComponents.EntitiesCount; i++)
            {
                if(!DeathComponents.Components1[i].Player.Equals(playerComponent)) continue;
                
                EcsWorld.RemoveEntity(DeathComponents.Entities[i]);
            }
        }
    }
}