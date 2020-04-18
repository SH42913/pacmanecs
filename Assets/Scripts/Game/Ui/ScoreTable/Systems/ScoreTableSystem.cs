using Game.Players;
using Leopotam.Ecs;

namespace Game.Ui.ScoreTable {
    public class ScoreTableSystem : IEcsRunSystem {
        private readonly EcsFilter<PlayerComponent> _players = null;
        private readonly EcsFilter<ScoreTableComponent> _scoreTables = null;
        private readonly EcsFilter<UpdateScoreTableEvent> _updateEvents = null;

        public void Run() {
            if (_updateEvents.IsEmpty()) return;

            string scoresString = "";
            foreach (int i in _players) {
                ref PlayerComponent player = ref _players.Get1(i);
                scoresString += $"P{player.Num.ToString()} Scores:{player.Scores.ToString()} ";

                if (player.IsDead) {
                    scoresString += "IS DEAD\n";
                }
                else {
                    scoresString += $"Lives:{player.Lives.ToString()}\n";
                }
            }

            foreach (int i in _scoreTables) {
                _scoreTables.Get1(i).ScoreText.text = scoresString;
            }
        }
    }
}