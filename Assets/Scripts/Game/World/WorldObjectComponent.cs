using Leopotam.Ecs;
using UnityEngine;

namespace Game.World
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