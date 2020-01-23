using Leopotam.Ecs;
using UnityEngine;

namespace Game.World
{
    public class CreateWorldObjectEvent : IEcsAutoReset
    {
        public Transform Transform;

        public void Reset()
        {
            Transform = null;
        }
    }
}