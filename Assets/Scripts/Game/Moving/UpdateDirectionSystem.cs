using Leopotam.Ecs;

namespace Game.Moving {
    public sealed class UpdateDirectionSystem : IEcsRunSystem {
        private readonly EcsFilter<MoveComponent, ChangeDirectionEvent> newDirectionEntities = null;

        public void Run() {
            foreach (var i in newDirectionEntities) {
                ref var move = ref newDirectionEntities.Get1(i);
                ref var changeDirection = ref newDirectionEntities.Get2(i);
                move.heading = changeDirection.newDirection;
            }
        }
    }
}