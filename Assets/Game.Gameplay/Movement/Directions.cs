using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Movement {
    public enum Directions {
        Up,
        Right,
        Down,
        Left,
    }

    public static class DirectionUtils {
        private static readonly Vector2Int[] offsets = {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

        private static readonly Quaternion[] rotations = {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, -90, 0),
        };

        public static readonly IReadOnlyList<Directions> availableDirections = (Directions[]) Enum.GetValues(typeof(Directions));

        public static Vector2Int GetPosition(this Directions direction, in Vector2Int position) {
            return position + GetPosition(direction);
        }

        public static Vector2Int GetPosition(this Directions direction) {
            return offsets[(int) direction];
        }

        public static Quaternion GetRotation(this Directions direction) {
            return rotations[(int) direction];
        }
    }
}