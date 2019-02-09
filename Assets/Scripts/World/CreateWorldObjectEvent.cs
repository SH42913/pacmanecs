using Leopotam.Ecs;
using UnityEngine;

namespace World
{
    [EcsOneFrame]
    public class CreateWorldObjectEvent : IEcsAutoResetComponent
    {
        public Transform Transform;

        public void Reset()
        {
            Transform = null;
        }
    }
}