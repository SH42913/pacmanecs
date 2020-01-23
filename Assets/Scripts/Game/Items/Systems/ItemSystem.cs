using Game.Players;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Items
{
    public class ItemSystem : IEcsRunSystem
    {
        private readonly WorldService _worldService = null;
        private readonly EcsFilter<NewPositionComponent, PlayerComponent> _players = null;

        public void Run()
        {
            foreach (int i in _players)
            {
                Vector2Int newPosition = _players.Get1[i].NewPosition;
                EcsEntity playerEntity = _players.Entities[i];

                foreach (EcsEntity entity in _worldService.WorldField[newPosition.x][newPosition.y])
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