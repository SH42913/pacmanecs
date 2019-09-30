using UnityEngine;

namespace Moving
{
    public enum Directions
    {
        Up,
        Right,
        Down,
        Left
    }

    public class MoveComponent
    {
        public float Speed;
        public Directions Heading;
        public Vector2Int DesiredPosition;
    }
}