using Leopotam.Ecs;
using UnityEngine;

namespace World.Systems
{
    [EcsInject]
    public class WorldSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        private readonly EcsFilter<WorldComponent> _world = null;
        private readonly EcsFilter<CreateWorldObjectEvent> _createEvents = null;
        private readonly EcsFilter<PositionComponent, NewPositionComponent> _movedObjects = null;
        private readonly EcsFilter<WorldObjectComponent, PositionComponent, DestroyedWorldObjectComponent> _destroyedObjects = null;

        public void Run()
        {
            WorldComponent world = _world.Components1[0];
            foreach (int i in _createEvents)
            {
                Transform newObject = _createEvents.Components1[i].Transform;
                EcsEntity entity = _createEvents.Entities[i];

                Vector2Int position = newObject.position.ToVector2Int();
                world.WorldField[position.x][position.y].Add(entity);
                _ecsWorld.AddComponent<PositionComponent>(entity).Position = position;
                _ecsWorld.AddComponent<WorldObjectComponent>(entity).Transform = newObject;
            }

            foreach (int i in _movedObjects)
            {
                PositionComponent positionComponent = _movedObjects.Components1[i];
                Vector2Int oldPosition = positionComponent.Position;
                Vector2Int newPosition = _movedObjects.Components2[i].NewPosition;
                EcsEntity entity = _movedObjects.Entities[i];

                world.WorldField[oldPosition.x][oldPosition.y].Remove(entity);
                world.WorldField[newPosition.x][newPosition.y].Add(entity);

                positionComponent.Position = newPosition;
            }

            foreach (int i in _destroyedObjects)
            {
                Transform objectToDestroy = _destroyedObjects.Components1[i].Transform;
                Vector2Int position = _destroyedObjects.Components2[i].Position;
                EcsEntity entity = _destroyedObjects.Entities[i];

                world.WorldField[position.x][position.y].Remove(entity);
                objectToDestroy.gameObject.SetActive(false);
                _ecsWorld.RemoveEntity(entity);
            }
        }
    }
}