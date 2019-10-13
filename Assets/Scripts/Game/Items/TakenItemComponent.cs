using Leopotam.Ecs;

namespace Game.Items
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