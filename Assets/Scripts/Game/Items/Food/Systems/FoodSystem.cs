using Game.Moving;
using Game.Players;
using Game.Ui.ScoreTable;
using Leopotam.Ecs;

namespace Game.Items.Food {
    public sealed class FoodSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<FoodComponent, TakenItemEvent> takenFoods = null;

        public void Run() {
            foreach (var i in takenFoods) {
                var playerEntity = takenFoods.Get2(i).playerEntity;
                if (!playerEntity.Has<PlayerComponent>()) continue;

                ref var player = ref playerEntity.Get<PlayerComponent>();
                player.scores += takenFoods.Get1(i).scores;

                ref var moveComponent = ref playerEntity.Get<MovementComponent>();
                moveComponent.speed -= takenFoods.Get1(i).speedPenalty;

                ecsWorld.NewEntity().Get<UpdateScoreTableEvent>();
            }
        }
    }
}