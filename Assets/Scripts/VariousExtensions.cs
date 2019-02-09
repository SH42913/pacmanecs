using System;
using UnityEngine;
using Random = System.Random;

public static class VariousExtensions
{
    public static Vector3 ToVector3(this Vector2Int vector, float height = 0)
    {
        return new Vector3(vector.x, height, vector.y);
    }

    public static Vector2Int ToVector2Int(this Vector3 vector)
    {
        return new Vector2Int
        {
            x = Mathf.RoundToInt(vector.x),
            y = Mathf.RoundToInt(vector.z),
        };
    }

    public static T NextFromArray<T>(this Random random, T[] array)
    {
        int index = random.Next(0, array.Length);
        return array[index];
    }

    public static T NextEnum<T>(this Random random)
    {
        var array = (T[]) Enum.GetValues(typeof(T));
        return random.NextFromArray(array);
    }
}