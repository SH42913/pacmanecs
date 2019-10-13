using Leopotam.Ecs;
using UnityEngine;

namespace Game.World
{
    public class NewPositionComponent : IEcsOneFrame
    {
        public Vector2Int NewPosition;
    }
}