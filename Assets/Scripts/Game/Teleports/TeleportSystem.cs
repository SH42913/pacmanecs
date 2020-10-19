using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Teleports {
    public sealed class TeleportSystem : IEcsRunSystem {
        private readonly EcsFilter<MovementComponent, WorldObjectComponent, TeleportedEvent> teleported = null;

        public void Run() {
            foreach (var i in teleported) {
                ref var moveComponent = ref teleported.Get1(i);
                var transform = teleported.Get2(i).transform;
                var targetPosition = teleported.Get3(i).newPosition;

                moveComponent.desiredPosition = targetPosition;
                transform.position = targetPosition.ToVector3(transform.position.y);

                teleported.GetEntity(i).Get<NewPositionEvent>().newPosition = targetPosition;
            }
        }
    }
}