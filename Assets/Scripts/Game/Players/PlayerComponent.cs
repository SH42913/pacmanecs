using UnityEngine;

namespace Game.Players {
    public struct PlayerComponent {
        public int num;
        public int scores;
        public int lives;
        public bool isDead;
        public Vector2Int spawnPosition;
    }
}