using Leopotam.Ecs;

namespace Game.Moving {
    public class UpdateDirectionSystem : IEcsRunSystem {
        private readonly EcsFilter<MoveComponent, ChangeDirectionEvent> _newDirectionEntities = null;

        public void Run() {
            foreach (int i in _newDirectionEntities) {
                ref MoveComponent move = ref _newDirectionEntities.Get1(i);
                ref ChangeDirectionEvent changeDirection = ref _newDirectionEntities.Get2(i);
                move.Heading = changeDirection.NewDirection;
            }
        }
    }
}