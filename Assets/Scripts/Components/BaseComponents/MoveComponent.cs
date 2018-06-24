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
        public Directions Heading { get; set; }
        public Vector3 DesiredPosition { get; set; }
        public float Speed { get; set; }
        public Transform Transform { get; set; }
    }
}