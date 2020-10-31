using UnityEngine;

namespace Game.Gameplay.Moving {
    public struct MovementComponent {
        public float speed;
        public Directions heading;
        public Vector2Int desiredPosition;
    }
}