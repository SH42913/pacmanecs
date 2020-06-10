using Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.World {
    public sealed class WorldSystem : IEcsRunSystem {
        private readonly WorldService worldService = null;

        private readonly EcsFilter<CreateWorldObjectEvent> createEvents = null;
        private readonly EcsFilter<PositionComponent, NewPositionEvent> movedObjects = null;
        private readonly EcsFilter<WorldObjectComponent, PositionComponent, DestroyedWorldObjectEvent> destroyedObjects = null;

        public void Run() {
            foreach (var i in createEvents) {
                var newObject = createEvents.Get1(i).transform;
                var entity = createEvents.GetEntity(i);

                var position = newObject.position.ToVector2Int();
                worldService.worldField[position.x][position.y].Add(entity);
                entity.Get<PositionComponent>().position = position;
                entity.Get<WorldObjectComponent>().transform = newObject;
            }

            foreach (var i in movedObjects) {
                ref var positionComponent = ref movedObjects.Get1(i);
                var oldPosition = positionComponent.position;
                var newPosition = movedObjects.Get2(i).newPosition;
                var entity = movedObjects.GetEntity(i);

                worldService.worldField[oldPosition.x][oldPosition.y].Remove(entity);
                worldService.worldField[newPosition.x][newPosition.y].Add(entity);

                positionComponent.position = newPosition;
            }

            foreach (var i in destroyedObjects) {
                var objectToDestroy = destroyedObjects.Get1(i).transform;
                var position = destroyedObjects.Get2(i).position;
                var entity = destroyedObjects.GetEntity(i);

                worldService.worldField[position.x][position.y].Remove(entity);
                objectToDestroy.gameObject.SetActive(false);
                entity.Destroy();
            }
        }
    }
}