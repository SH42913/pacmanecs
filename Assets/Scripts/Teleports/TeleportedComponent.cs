using Leopotam.Ecs;
using UnityEngine;

namespace Teleports
{
    public class TeleportedComponent : IEcsOneFrame
    {
        public Vector2Int NewPosition;
    }
}