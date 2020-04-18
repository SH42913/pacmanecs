using UnityEngine;

namespace Game.Moving {
    public enum Directions {
        Up,
        Right,
        Down,
        Left
    }

    public struct MoveComponent {
        public float Speed;
        public Directions Heading;
        public Vector2Int DesiredPosition;
    }
}