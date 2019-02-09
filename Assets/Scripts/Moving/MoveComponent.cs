using UnityEngine;

namespace Moving
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
        public float Speed;
        public Vector2Int DesiredPosition;
    }
}