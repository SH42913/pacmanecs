using Leopotam.Ecs;
using Moving;
using UnityEngine;
using World;

namespace Teleports.Systems
{
    public class TeleportSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MoveComponent, WorldObjectComponent, TeleportedComponent> _teleported = null;

        public void Run()
        {
            foreach (int i in _teleported)
            {
                MoveComponent moveComponent = _teleported.Get1[i];
                Transform transform = _teleported.Get2[i].Transform;
                Vector2Int targetPosition = _teleported.Get3[i].NewPosition;
                EcsEntity entity = _teleported.Entities[i];

                moveComponent.DesiredPosition = targetPosition;
                transform.position = targetPosition.ToVector3(transform.position.y);

                entity.Set<NewPositionComponent>().NewPosition = targetPosition;
            }
        }
    }
}