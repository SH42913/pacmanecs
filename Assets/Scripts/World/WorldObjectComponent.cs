using Leopotam.Ecs;
using UnityEngine;

namespace World
{
    public class WorldObjectComponent : IEcsAutoReset
    {
        public Transform Transform;

        public void Reset()
        {
            Transform = null;
        }
    }
}