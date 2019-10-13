using Leopotam.Ecs;

namespace Game.Moving
{
    public class ChangeDirectionComponent : IEcsOneFrame
    {
        public Directions NewDirection;
    }
}