using Game.Moving;
using Game.Players;
using Game.Ui.ScoreTable;
using Leopotam.Ecs;

namespace Game.Items.Food
{
    public class FoodSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly EcsFilter<FoodComponent, TakenItemComponent> _takenFoods = null;

        public void Run()
        {
            foreach (int i in _takenFoods)
            {
                EcsEntity playerEntity = _takenFoods.Get2[i].PlayerEntity;
                var player = playerEntity.Get<PlayerComponent>();
                var moveComponent = playerEntity.Get<MoveComponent>();

                player.Scores += _takenFoods.Get1[i].Scores;
                moveComponent.Speed -= _takenFoods.Get1[i].SpeedPenalty;

                _ecsWorld.NewEntityWith(out UpdateScoreTableEvent _);
            }
        }
    }
}