using Leopotam.Ecs;

namespace Moving
{
    [EcsOneFrame]
    public class ChangeDirectionComponent
    {
        public Directions NewDirection;
    }
}