using UnityEngine;

namespace Game.Players {
    public struct PlayerComponent {
        public int Num;
        public int Scores;
        public int Lives;
        public bool IsDead;
        public Vector2Int SpawnPosition;
    }
}