using System;
using Components.BaseComponents;
using Components.ItemComponents;
using Components.PlayerComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.ItemSystems
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

                var item = _items.GetSecondComponent(x => x.Position == playerPosition);
                if(item == null) continue;
                
                item.GameObject.SetActive(false);
                int playerEntity = _players.Entities[i];
                switch (item.ItemType)
                {
                    case ItemTypes.Food:
                        CreateFoodComponent(playerEntity);
                        break;
                    case ItemTypes.Energizer:
                        CreateEnergizerComponent(playerEntity);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _ecsWorld.RemoveEntity(item.ItemEntity);
            }
        }

        public void Destroy() {}

        private void CreateFood()
        {
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");

            foreach (GameObject foodObject in foodObjects)
            {
                int entity = foodObject.CreateEntityWithPosition(_ecsWorld);
                
                var foodComponent = _ecsWorld.AddComponent<ItemComponent>(entity);
                foodComponent.ItemType = ItemTypes.Food;
                foodComponent.GameObject = foodObject;
                foodComponent.ItemEntity = entity;
            }
        }

        private void CreateEnergizers()
        {
            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");

            foreach (GameObject energizer in energizers)
            {
                PositionComponent position;
                int entity = _ecsWorld.CreateEntityWith(out position);
                position.Position = energizer.transform.position.ToVector2Int();

                var component = _ecsWorld.AddComponent<ItemComponent>(entity);
                component.ItemType = ItemTypes.Energizer;
                component.GameObject = energizer;
                component.ItemEntity = entity;
            }
        }

        private void CreateFoodComponent(int playerEntity)
        {
            var component = _ecsWorld.CreateEntityWith<FoodComponent>();
            component.PlayerEntity = playerEntity;
            component.SpeedPenalty = FoodPenalty;
            component.Scores = 10;
        }

        private void CreateEnergizerComponent(int playerEntity)
        {
            var component = _ecsWorld.CreateEntityWith<EnergizerComponent>();
            component.PlayerEntity = playerEntity;
            component.SpeedPenalty = 5 * FoodPenalty;
            component.Scores = 50;
        }
    }
}