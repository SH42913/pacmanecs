using UnityEngine;

namespace Components
{
    public class PlayerComponent
    {
        public uint Num { get; set; }
        public uint Scores { get; set; }
        public uint Lifes { get; set; }
        public bool IsDead { get; set; }
        public Vector3 StartPosition { get; set; }
    }
}