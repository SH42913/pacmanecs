using Leopotam.Ecs;
using UnityEngine;

namespace World
{
    public class CreateWorldObjectEvent : IEcsOneFrame, IEcsAutoReset
    {
        public Transform Transform;

        public void Reset()
        {
            Transform = null;
        }
    }
}