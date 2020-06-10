using Utils;
using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Teleports {
    public class TeleportSystem : IEcsRunSystem {
        private readonly EcsFilter<MoveComponent, WorldObjectComponent, TeleportedEvent> _teleported = null;

        public void Run() {
            foreach (int i in _teleported) {
                ref MoveComponent moveComponent = ref _teleported.Get1(i);
                Transform transform = _teleported.Get2(i).Transform;
                Vector2Int targetPosition = _teleported.Get3(i).NewPosition;

                moveComponent.DesiredPosition = targetPosition;
                transform.position = targetPosition.ToVector3(transform.position.y);

                _teleported.GetEntity(i).Get<NewPositionEvent>().NewPosition = targetPosition;
            }
        }
    }
}