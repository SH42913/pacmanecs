using Leopotam.Ecs;

namespace Game.Moving
{
    public class UpdateDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MoveComponent, ChangeDirectionEvent> _newDirectionEntities = null;

        public void Run()
        {
            foreach (int i in _newDirectionEntities)
            {
                MoveComponent move = _newDirectionEntities.Get1[i];
                ChangeDirectionEvent changeDirection = _newDirectionEntities.Get2[i];
                move.Heading = changeDirection.NewDirection;
            }
        }
    }
}