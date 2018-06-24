using UnityEngine;

namespace Components.BaseComponents
{
    public class TeleportComponent
    {
        public MoveComponent MoveComponent { get; set; }
        public Vector3 TargetPosition { get; set; }
    }
}