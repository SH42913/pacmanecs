using UnityEngine;

namespace Game.Moving {
    public enum Directions {
        Up,
        Right,
        Down,
        Left
    }

    public struct MoveComponent {
        public float speed;
        public Directions heading;
        public Vector2Int desiredPosition;
    }
}