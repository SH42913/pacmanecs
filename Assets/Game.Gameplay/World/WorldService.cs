using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.World {
    public sealed class WorldService {
        public HashSet<EcsEntity>[][] worldField;

        public HashSet<EcsEntity> GetEntitiesOn(in Vector2Int position) => worldField[position.x][position.y];
    }
}