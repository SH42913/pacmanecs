using Game.Moving;
using Game.World;
using Leopotam.Ecs;
using Utils;

namespace Game.Teleports {
    public sealed class TeleportSystem : IEcsRunSystem {
        private readonly EcsFilter<MovementComponent, WorldObjectComponent, TeleportToPositionRequest> requests = null;

        public void Run() {
            foreach (var i in requests) {
                ref var moveComponent = ref requests.Get1(i);
                var transform = requests.Get2(i).transform;
                var targetPosition = requests.Get3(i).newPosition;

                moveComponent.desiredPosition = targetPosition;
                transform.position = targetPosition.ToVector3(transform.position.y);

                requests.GetEntity(i).Get<WorldObjectNewPositionRequest>().newPosition = targetPosition;
            }
        }
    }
}