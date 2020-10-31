using System;
using UnityEngine;

namespace Game.Gameplay {
    internal static class VariousExtensions {
        public static Vector3 ToVector3(this Vector2Int vector, float height = 0) => new Vector3(vector.x, height, vector.y);

        public static Vector2Int ToVector2Int(this Vector3 vector) => new Vector2Int {
            x = Mathf.RoundToInt(vector.x),
            y = Mathf.RoundToInt(vector.z),
        };

        public static T NextFromArray<T>(this System.Random random, T[] array) {
            var index = random.Next(0, array.Length);
            return array[index];
        }

        public static T NextEnum<T>(this System.Random random) {
            var array = (T[]) Enum.GetValues(typeof(T));
            return random.NextFromArray(array);
        }
    }
}