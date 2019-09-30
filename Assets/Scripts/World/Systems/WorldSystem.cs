using Leopotam.Ecs;
using UnityEngine;

namespace World.Systems
{
    public class WorldSystem : IEcsRunSystem
    {
        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<CreateWorldObjectEvent> _createEvents = null;
        private readonly EcsFilter<PositionComponent, NewPositionComponent> _movedObjects = null;
        private readonly EcsFilter<WorldObjectComponent, PositionComponent, DestroyedWorldObjectComponent> _destroyedObjects = null;

        public void Run()
        {
            WorldComponent world = _world.Get1[0];
            foreach (int i in _createEvents)
            {
                Transform newObject = _createEvents.Get1[i].Transform;
                EcsEntity entity = _createEvents.Entities[i];

                Vector2Int position = newObject.position.ToVector2Int();
                world.WorldField[position.x][position.y].Add(entity);
                entity.Set<PositionComponent>().Position = position;
                entity.Set<WorldObjectComponent>().Transform = newObject;
            }

            foreach (int i in _movedObjects)
            {
                PositionComponent positionComponent = _movedObjects.Get1[i];
                Vector2Int oldPosition = positionComponent.Position;
                Vector2Int newPosition = _movedObjects.Get2[i].NewPosition;
                EcsEntity entity = _movedObjects.Entities[i];

                world.WorldField[oldPosition.x][oldPosition.y].Remove(entity);
                world.WorldField[newPosition.x][newPosition.y].Add(entity);

                positionComponent.Position = newPosition;
            }

            foreach (int i in _destroyedObjects)
            {
                Transform objectToDestroy = _destroyedObjects.Get1[i].Transform;
                Vector2Int position = _destroyedObjects.Get2[i].Position;
                EcsEntity entity = _destroyedObjects.Entities[i];

                world.WorldField[position.x][position.y].Remove(entity);
                objectToDestroy.gameObject.SetActive(false);
                entity.Destroy();
            }
        }
    }
}