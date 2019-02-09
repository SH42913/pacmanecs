using Leopotam.Ecs;
using UnityEngine;

namespace Teleports
{
    [EcsOneFrame]
    public class TeleportingComponent
    {
        public Vector2Int NewPosition;
    }
}