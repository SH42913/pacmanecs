using Leopotam.Ecs;

namespace Moving
{
    public class ChangeDirectionComponent : IEcsOneFrame
    {
        public Directions NewDirection;
    }
}