using Leopotam.Ecs;

namespace Game.Gameplay.World {
    public sealed class WorldSystem : IEcsRunSystem {
        private readonly WorldService worldService = null;

        private readonly EcsFilter<WorldObjectCreateRequest> createRequests = null;
        private readonly EcsFilter<PositionComponent, WorldObjectNewPositionRequest> positionRequests = null;
        private readonly EcsFilter<WorldObjectComponent, PositionComponent, WorldObjectDestroyedEvent> destroyedObjects = null;

        public void Run() {
            foreach (var i in createRequests) {
                var newObject = createRequests.Get1(i).transform;
                var entity = createRequests.GetEntity(i);

                var position = newObject.position.ToVector2Int();
                worldService.worldField[position.x][position.y].Add(entity);
                entity.Get<PositionComponent>().position = position;
                entity.Get<WorldObjectComponent>().transform = newObject;
            }

            foreach (var i in positionRequests) {
                ref var positionComponent = ref positionRequests.Get1(i);
                var oldPosition = positionComponent.position;
                var newPosition = positionRequests.Get2(i).newPosition;
                var entity = positionRequests.GetEntity(i);

                worldService.worldField[oldPosition.x][oldPosition.y].Remove(entity);
                worldService.worldField[newPosition.x][newPosition.y].Add(entity);

                positionComponent.position = newPosition;
            }

            foreach (var i in destroyedObjects) {
                var objectToDestroy = destroyedObjects.Get1(i).transform;
                var position = destroyedObjects.Get2(i).position;
                var deleteEntity = destroyedObjects.Get3(i).deleteEntity;
                var entity = destroyedObjects.GetEntity(i);

                worldService.worldField[position.x][position.y].Remove(entity);
                objectToDestroy.gameObject.SetActive(false);

                if (deleteEntity) {
                    entity.Destroy();
                } else {
                    entity.Del<WorldObjectComponent>();
                    entity.Del<PositionComponent>();
                }
            }
        }
    }
}