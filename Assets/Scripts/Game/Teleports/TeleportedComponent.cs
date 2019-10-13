using Leopotam.Ecs;
using UnityEngine;

namespace Game.Teleports
{
    public class TeleportedComponent : IEcsOneFrame
    {
        public Vector2Int NewPosition;
    }
}