using Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.World
{
    public class WorldSystem : IEcsRunSystem
    {
        private readonly WorldService _worldService = null;
        private readonly EcsFilter<CreateWorldObjectEvent> _createEvents = null;
        private readonly EcsFilter<PositionComponent, NewPositionEvent> _movedObjects = null;
        private readonly EcsFilter<WorldObjectComponent, PositionComponent, DestroyedWorldObjectEvent> _destroyedObjects = null;

        public void Run()
        {
            foreach (int i in _createEvents)
            {
                Transform newObject = _createEvents.Get1[i].Transform;
                EcsEntity entity = _createEvents.Entities[i];

                Vector2Int position = newObject.position.ToVector2Int();
                _worldService.WorldField[position.x][position.y].Add(entity);
                entity.Set<PositionComponent>().Position = position;
                entity.Set<WorldObjectComponent>().Transform = newObject;
            }

            foreach (int i in _movedObjects)
            {
                PositionComponent positionComponent = _movedObjects.Get1[i];
                Vector2Int oldPosition = positionComponent.Position;
                Vector2Int newPosition = _movedObjects.Get2[i].NewPosition;
                EcsEntity entity = _movedObjects.Entities[i];

                _worldService.WorldField[oldPosition.x][oldPosition.y].Remove(entity);
                _worldService.WorldField[newPosition.x][newPosition.y].Add(entity);

                positionComponent.Position = newPosition;
            }

            foreach (int i in _destroyedObjects)
            {
                Transform objectToDestroy = _destroyedObjects.Get1[i].Transform;
                Vector2Int position = _destroyedObjects.Get2[i].Position;
                EcsEntity entity = _destroyedObjects.Entities[i];

                _worldService.WorldField[position.x][position.y].Remove(entity);
                objectToDestroy.gameObject.SetActive(false);
                entity.Destroy();
            }
        }
    }
}