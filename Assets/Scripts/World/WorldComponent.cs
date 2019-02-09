using System.Collections.Generic;
using Leopotam.Ecs;

namespace World
{
    public class WorldComponent : IEcsAutoResetComponent
    {
        public HashSet<int>[][] WorldField;

        public void Reset()
        {
            WorldField = null;
        }
    }
}