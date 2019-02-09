using Leopotam.Ecs;

namespace Moving.Systems
{
    [EcsInject]
    public class UpdateDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MoveComponent, ChangeDirectionComponent> _newDirectionEntities = null;

        public void Run()
        {
            foreach (int i in _newDirectionEntities)
            {
                MoveComponent move = _newDirectionEntities.Components1[i];
                ChangeDirectionComponent changeDirection = _newDirectionEntities.Components2[i];
                move.Heading = changeDirection.NewDirection;
            }
        }
    }
}