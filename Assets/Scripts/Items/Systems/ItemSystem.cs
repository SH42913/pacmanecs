using Leopotam.Ecs;
using Players;
using UnityEngine;
using World;

namespace Items.Systems
{
    public class ItemSystem : IEcsRunSystem
    {
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<NewPositionComponent, PlayerComponent> _players = null;

        public void Run()
        {
            WorldComponent world = _world.Get1[0];
            foreach (int i in _players)
            {
                Vector2Int newPosition = _players.Get1[i].NewPosition;
                EcsEntity playerEntity = _players.Entities[i];

                foreach (EcsEntity entity in world.WorldField[newPosition.x][newPosition.y])
                {
                    var item = entity.Get<ItemComponent>();
                    if (item == null) continue;

                    entity.Set<TakenItemComponent>().PlayerEntity = playerEntity;
                    entity.Set<DestroyedWorldObjectComponent>();
                }
            }
        }
    }
}