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
        
        public void Run()
        {
            for (int i = 0; i < _deathComponents.EntitiesCount; i++)
            {
                int playerEntity = _deathComponents.Components1[i].PlayerEntity;
                var targetPlayer = _ecsWorld.GetComponent<PlayerComponent>(playerEntity);
                var deadMoveComponent = _ecsWorld.GetComponent<MoveComponent>(playerEntity);
                
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
                teleportComponent.MoveEntity = playerEntity;
                teleportComponent.TargetPosition = respawnVector;
                
                _ecsWorld.CreateEntityWith<UpdateGuiComponent>();
                RemoveEntitiesWith(playerEntity);
            }
        }

        private void RemoveEntitiesWith(int playerEntity)
        {
            for (int i = 0; i < _deathComponents.EntitiesCount; i++)
            {
                if(_deathComponents.Components1[i].PlayerEntity != playerEntity) continue;
                _ecsWorld.RemoveEntity(_deathComponents.Entities[i]);
            }
        }
    }
}