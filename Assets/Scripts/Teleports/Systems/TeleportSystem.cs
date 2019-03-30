using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;

namespace Teleports.Systems
{
    [EcsInject]
    public class TeleportSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<MoveComponent, WorldObjectComponent, TeleportingComponent> _teleportedEntities = null;

        public void Run()
        {
            foreach (int i in _teleportedEntities)
            {
                MoveComponent moveComponent = _teleportedEntities.Components1[i];
                Transform transform = _teleportedEntities.Components2[i].Transform;
                Vector2Int targetPosition = _teleportedEntities.Components3[i].NewPosition;
                int entity = _teleportedEntities.Entities[i];

                moveComponent.DesiredPosition = targetPosition;
                transform.position = targetPosition.ToVector3(transform.position.y);

                _ecsWorld.EnsureComponent<NewPositionComponent>(entity, out _).NewPosition = targetPosition;
            }
        }
    }
}