using UnityEngine;

namespace Components.BaseComponents
{
    public enum Directions
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    
    public class MoveComponent
    {
        public Directions Heading;
        public Vector3 DesiredPosition;
        public float Speed;
        public Transform Transform;
    }
}