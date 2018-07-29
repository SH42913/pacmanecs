using Components.BaseComponents;
using Components.PlayerComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class DeathSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<DeathComponent> _deathComponents = null;
        private EcsFilter<MoveComponent, PlayerComponent> _players = null;
        
        public void Run()
        {
            for (int i = 0; i < _deathComponents.EntitiesCount; i++)
            {
                var targetPlayer = _deathComponents.Components1[i].Player;
                MoveComponent deadMoveComponent = _players.GetFirstComponent(null, x => x.Equals(targetPlayer));
                
                if (--targetPlayer.Lifes <= 0)
                {
                    targetPlayer.IsDead = true;
                    if (deadMoveComponent != null)
                    {
                        deadMoveComponent.Transform.gameObject.SetActive(false);
                    }
                }
                
                Vector3 respawnVector = targetPlayer.Lifes > 0
                    ? targetPlayer.StartPosition
                    : Vector3.zero;

                var teleportComponent = _ecsWorld.CreateEntityWith<TeleportComponent>();
                teleportComponent.MoveComponent = deadMoveComponent;
                teleportComponent.TargetPosition = respawnVector;
                
                _ecsWorld.CreateEntityWith<UpdateGuiComponent>();
                RemoveEntitiesWith(targetPlayer);
            }
        }

        private void RemoveEntitiesWith(PlayerComponent playerComponent)
        {
            for (int i = 0; i < _deathComponents.EntitiesCount; i++)
            {
                if(!_deathComponents.Components1[i].Player.Equals(playerComponent)) continue;
                
                _ecsWorld.RemoveEntity(_deathComponents.Entities[i]);
            }
        }
    }
}