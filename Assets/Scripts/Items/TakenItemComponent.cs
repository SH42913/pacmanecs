using Leopotam.Ecs;

namespace Items
{
    public class TakenItemComponent : IEcsOneFrame, IEcsAutoReset
    {
        public EcsEntity PlayerEntity;

        public void Reset()
        {
            PlayerEntity = EcsEntity.Null;
        }
    }
}