using UnityEngine;

namespace Game.Moving {
    public struct MoveComponent {
        public float speed;
        public Directions heading;
        public Vector2Int desiredPosition;
    }
}