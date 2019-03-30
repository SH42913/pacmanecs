using Leopotam.Ecs;
using Players;
using UnityEngine;
using World;

namespace Items.Systems
{
    [EcsInject]
    public class ItemSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<NewPositionComponent, PlayerComponent> _players = null;

        public void Run()
        {
            WorldComponent world = _world.Components1[0];
            foreach (int i in _players)
            {
                Vector2Int newPosition = _players.Components1[i].NewPosition;
                EcsEntity playerEntity = _players.Entities[i];

                foreach (EcsEntity entity in world.WorldField[newPosition.x][newPosition.y])
                {
                    var item = _ecsWorld.GetComponent<ItemComponent>(entity);
                    if (item == null) continue;

                    _ecsWorld.AddComponent<TakenItemComponent>(entity).PlayerEntity = playerEntity;
                    _ecsWorld.AddComponent<DestroyedWorldObjectComponent>(entity);
                }
            }
        }
    }
}