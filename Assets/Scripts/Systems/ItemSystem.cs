using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class ItemSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<PositionComponent, ItemComponent> Items { get; set; }
        private EcsFilter<PositionComponent, MoveComponent, PlayerComponent> Players { get; set; }
        
        public void Initialize()
        {
            CreateFood();
            CreateEnergizers();
            EcsWorld.CreateEntityWith<UpdateGuiComponent>();
        }

        public void Run()
        {
            for (int i = 0; i < Players.EntitiesCount; i++)
            {
                var playerPosition = Players.Components1[i].Position;

                var item = Items.GetSecondComponent(
                    x => x.Position == playerPosition,
                    x => !x.Used);
                if(item == null) continue;
                
                item.UseAction.Invoke(Players.Components2[i], Players.Components3[i]);
                item.Used = true;
                item.GameObject.SetActive(false);
                EcsWorld.CreateEntityWith<UpdateGuiComponent>();
            }
        }

        public void Destroy() {}

        private void CreateFood()
        {
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");

            foreach (var foodObject in foodObjects)
            {
                int entity = EcsWorld.CreateEntity();
                EcsWorld
                    .AddComponent<PositionComponent>(entity)
                    .Position = foodObject.transform.position.ToVector2Int();

                var foodComponent = EcsWorld.AddComponent<ItemComponent>(entity);
                foodComponent.ItemType = ItemTypes.Food;
                foodComponent.UseAction = FoodAction;
                foodComponent.GameObject = foodObject;
            }
        }

        private void CreateEnergizers()
        {
            GameObject[] energizers = GameObject.FindGameObjectsWithTag("Energizer");

            foreach (var energizer in energizers)
            {
                int entity = EcsWorld.CreateEntity();
                EcsWorld
                    .AddComponent<PositionComponent>(entity)
                    .Position = energizer.transform.position.ToVector2Int();

                var component = EcsWorld.AddComponent<ItemComponent>(entity);
                component.ItemType = ItemTypes.Energizer;
                component.UseAction = EnergizerAction;
                component.GameObject = energizer;
            }
        }

        private void FoodAction(MoveComponent moveComponent, PlayerComponent player)
        {
            player.Scores += 10;
            moveComponent.Speed -= 0.001f;
        }

        private void EnergizerAction(MoveComponent moveComponent, PlayerComponent player)
        {
            player.Scores += 50;
            moveComponent.Speed -= 0.005f;
        }
    }
}