using UnityEngine;

namespace Components.PlayerComponents
{
    public class PlayerComponent
    {
        public int Num { get; set; }
        public int Scores { get; set; }
        public int Lifes { get; set; }
        public bool IsDead { get; set; }
        public Vector3 StartPosition { get; set; }
    }
}