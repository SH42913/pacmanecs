using Leopotam.Ecs;

namespace Moving.Systems
{
    public class UpdateDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MoveComponent, ChangeDirectionComponent> _newDirectionEntities = null;

        public void Run()
        {
            foreach (int i in _newDirectionEntities)
            {
                MoveComponent move = _newDirectionEntities.Get1[i];
                ChangeDirectionComponent changeDirection = _newDirectionEntities.Get2[i];
                move.Heading = changeDirection.NewDirection;
            }
        }
    }
}