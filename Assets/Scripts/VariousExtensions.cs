using System;
using System.Linq.Expressions;
using Components;
using LeopotamGroup.Ecs;
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
        int entity = ecsWorld.CreateEntity();
        ecsWorld
            .AddComponent<PositionComponent>(entity)
            .Position = gameObject.transform.position.ToVector2Int();

        return entity;
    }

    public static T GetComponent<T>(this EcsFilter<T> filter, Expression<Func<T, bool>> limits = null)
        where T : class, new()
    {
        if(limits == null) limits = x => true;
        var func = limits.Compile();

        for (int i = 0; i < filter.EntitiesCount; i++)
        {
            var component = filter.Components1[i];
            if(!func(component)) continue;

            return component;
        }

        return null;
    }
    
    public static T1 GetFirstComponent<T1, T2>(
        this EcsFilter<T1, T2> filter,
        Expression<Func<T1, bool>> limits1 = null,
        Expression<Func<T2, bool>> limits2 = null)
        where T1 : class, new()
        where T2 : class, new()
    {
        if(limits1 == null) limits1 = x => true;
        if(limits2 == null) limits2 = x => true;
        
        var func1 = limits1.Compile();
        var func2 = limits2.Compile();

        for (int i = 0; i < filter.EntitiesCount; i++)
        {
            var component1 = filter.Components1[i];
            var component2 = filter.Components2[i];
            if(!func1(component1) || !func2(component2)) continue;

            return component1;
        }

        return null;
    }
    
    public static T2 GetSecondComponent<T1, T2>(
        this EcsFilter<T1, T2> filter,
        Expression<Func<T1, bool>> limits1 = null,
        Expression<Func<T2, bool>> limits2 = null)
        where T1 : class, new()
        where T2 : class, new()
    {
        if(limits1 == null) limits1 = x => true;
        if(limits2 == null) limits2 = x => true;
        
        var func1 = limits1.Compile();
        var func2 = limits2.Compile();

        for (int i = 0; i < filter.EntitiesCount; i++)
        {
            var component1 = filter.Components1[i];
            var component2 = filter.Components2[i];
            if(!func1(component1) || !func2(component2)) continue;

            return component2;
        }

        return null;
    }
}