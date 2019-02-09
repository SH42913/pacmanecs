using Leopotam.Ecs;
using UnityEngine;

namespace World
{
    public class WorldObjectComponent : IEcsAutoResetComponent
    {
        public Transform Transform;

        public void Reset()
        {
            Transform = null;
        }
    }
}