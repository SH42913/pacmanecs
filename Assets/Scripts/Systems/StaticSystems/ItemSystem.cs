using Components.BaseComponents;
using Components.PlayerComponents;
using Components.StaticComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.StaticSystems
{
    [EcsInject]
    public class ItemSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float FoodPenalty;
        
        private EcsWorld _ecsWorld = null;
        private EcsFilter<PositionComponent, ItemComponent> _items = null;
        private EcsFilter<PositionComponent, MoveComponent, PlayerComponent> _players = null;

        public void Initialize()
        {
            CreateFood();
            CreateEnergizers();
            _ecsWorld.CreateEntityWith<UpdateGuiComponent>();
        }

        public void Run()
        {
            for (int i = 0; i < _players.EntitiesCount; i++)
            {
                var playerPosition = _players.Components1[i].Position;

                var item = _items.GetSecondComponent(
                    x => x.Position == playerPosition,
                    x => !x.Used);
                if(item == null) continue;
                
                item.UseAction(_players.Components2[i], _players.Components3[i]);
                item.Used = true;
                item.GameObject.SetActive(false);
                _ecsWorld.CreateEntityWith<UpdateGuiComponent>();
                _ecsWorld.RemoveEntity(item.ItemEntity);
            }
        }

        public void Destroy() {}

        private void CreateFood()
        {
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");

            foreach (var foodObject in foodObjects)
            {
                int entity = foodObject.CreateEntityWithPosition(_ecsWorld);
                
                var foodComponent = _ecsWorld.AddComponent<ItemComponent>(entity);
                foodComponent.ItemType = ItemTypes.Food;
                foodComponent.UseAction = FoodAction;
                foodComponent.GameObject = foodObject;
                foodComponent.ItemEntity = entity;
            }
        }

        private void CreateEnergizers()
        {
            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");

            foreach (var energizer in energizers)
            {
                int entity = _ecsWorld.CreateEntity();
                _ecsWorld
                    .AddComponent<PositionComponent>(entity)
                    .Position = energizer.transform.position.ToVector2Int();

                var component = _ecsWorld.AddComponent<ItemComponent>(entity);
                component.ItemType = ItemTypes.Energizer;
                component.UseAction = EnergizerAction;
                component.GameObject = energizer;
                component.ItemEntity = entity;
            }
        }

        private void FoodAction(MoveComponent moveComponent, PlayerComponent player)
        {
            player.Scores += 10;
            moveComponent.Speed -= FoodPenalty;
        }

        private void EnergizerAction(MoveComponent moveComponent, PlayerComponent player)
        {
            player.Scores += 50;
            moveComponent.Speed -= 5 * FoodPenalty;
        }
    }
}