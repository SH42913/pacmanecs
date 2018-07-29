using System;
using Components.BaseComponents;
using Leopotam.Ecs;
using UnityEngine;

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

    public static int CreateEntityWithPosition(this GameObject gameObject, EcsWorld ecsWorld)
    {
        PositionComponent position;
        int entity = ecsWorld.CreateEntityWith(out position);
        position.Position = gameObject.transform.position.ToVector2Int();

        return entity;
    }

    /// <summary>
    /// You better don't use this extensions in real projects, 'cause it can strike on performance
    /// </summary>
    public static T GetComponent<T>(this EcsFilter<T> filter, Func<T, bool> limits = null)
        where T : class, new()
    {
        if(limits == null) limits = x => true;

        for (int i = 0; i < filter.EntitiesCount; i++)
        {
            var component = filter.Components1[i];
            if(!limits(component)) continue;

            return component;
        }

        return null;
    }
    
    /// <summary>
    /// You better don't use this extensions in real projects, 'cause it can strike on performance
    /// </summary>
    public static T1 GetFirstComponent<T1, T2>(
        this EcsFilter<T1, T2> filter,
        Func<T1, bool> limits1 = null,
        Func<T2, bool> limits2 = null)
        where T1 : class, new()
        where T2 : class, new()
    {
        if(limits1 == null) limits1 = x => true;
        if(limits2 == null) limits2 = x => true;

        for (int i = 0; i < filter.EntitiesCount; i++)
        {
            var component1 = filter.Components1 != null
                ? filter.Components1[i]
                : null;
            var component2 = filter.Components2 != null
                ? filter.Components2[i]
                : null;
            if(!limits1(component1) || !limits2(component2)) continue;

            return component1;
        }

        return null;
    }
    
    /// <summary>
    /// You better don't use this extensions in real projects, 'cause it can strike on performance
    /// </summary>
    public static T2 GetSecondComponent<T1, T2>(
        this EcsFilter<T1, T2> filter,
        Func<T1, bool> limits1 = null,
        Func<T2, bool> limits2 = null)
        where T1 : class, new()
        where T2 : class, new()
    {
        if(limits1 == null) limits1 = x => true;
        if(limits2 == null) limits2 = x => true;

        for (int i = 0; i < filter.EntitiesCount; i++)
        {
            var component1 = filter.Components1 != null
                ? filter.Components1[i]
                : null;
            var component2 = filter.Components2 != null
                ? filter.Components2[i]
                : null;
            if(!limits1(component1) || !limits2(component2)) continue;

            return component2;
        }

        return null;
    }
}