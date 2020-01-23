using Leopotam.Ecs;

namespace Game.Items
{
    public class TakenItemEvent : IEcsAutoReset
    {
        public EcsEntity PlayerEntity;

        public void Reset()
        {
            PlayerEntity = EcsEntity.Null;
        }
    }
}