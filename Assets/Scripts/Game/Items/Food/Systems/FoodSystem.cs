using Game.Moving;
using Game.Players;
using Game.Ui.ScoreTable;
using Leopotam.Ecs;

namespace Game.Items.Food {
    public class FoodSystem : IEcsRunSystem {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<FoodComponent, TakenItemEvent> _takenFoods = null;

        public void Run() {
            foreach (int i in _takenFoods) {
                EcsEntity playerEntity = _takenFoods.Get2(i).PlayerEntity;
                if (!playerEntity.Has<PlayerComponent>()) continue;

                ref var player = ref playerEntity.Set<PlayerComponent>();
                ref var moveComponent = ref playerEntity.Set<MoveComponent>();

                player.Scores += _takenFoods.Get1(i).Scores;
                moveComponent.Speed -= _takenFoods.Get1(i).SpeedPenalty;

                _ecsWorld.NewEntity().Set<UpdateScoreTableEvent>();
            }
        }
    }
}