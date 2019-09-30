using System.Collections.Generic;
using Leopotam.Ecs;

namespace World
{
    public class WorldComponent : IEcsAutoReset
    {
        public HashSet<EcsEntity>[][] WorldField;

        public void Reset()
        {
            WorldField = null;
        }
    }
}