using Game.Gameplay.Movement;
using Game.Gameplay.Players;
using Leopotam.Ecs;

namespace Game.Gameplay.Items.Food {
    public sealed class FoodSystem : IEcsRunSystem {
        private readonly EcsFilter<FoodComponent, ItemTakenEvent> takenFoods = null;

        public void Run() {
            foreach (var i in takenFoods) {
                var playerEntity = takenFoods.Get2(i).playerEntity;
                if (!playerEntity.Has<PlayerComponent>()) continue;

                ref var player = ref playerEntity.Get<PlayerComponent>();
                player.scores += takenFoods.Get1(i).scores;

                ref var moveComponent = ref playerEntity.Get<MovementComponent>();
                moveComponent.speed -= takenFoods.Get1(i).speedPenalty;

                playerEntity.Get<PlayerScoreChangedEvent>();
            }
        }
    }
}