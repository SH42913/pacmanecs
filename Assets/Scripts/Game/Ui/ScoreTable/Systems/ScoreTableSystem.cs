using Game.Players;
using Leopotam.Ecs;

namespace Game.Ui.ScoreTable {
    public sealed class ScoreTableSystem : IEcsRunSystem {
        private readonly EcsFilter<PlayerComponent> players = null;
        private readonly EcsFilter<ScoreTableComponent> scoreTables = null;
        private readonly EcsFilter<UpdateScoreTableEvent> updateEvents = null;

        public void Run() {
            if (updateEvents.IsEmpty()) return;

            var scoresString = "";
            foreach (var i in players) {
                ref var player = ref players.Get1(i);
                scoresString += $"P{player.num.ToString()} Scores:{player.scores.ToString()} ";

                if (player.isDead) {
                    scoresString += "IS DEAD\n";
                } else {
                    scoresString += $"Lives:{player.lives.ToString()}\n";
                }
            }

            foreach (var i in scoreTables) {
                scoreTables.Get1(i).scoreText.text = scoresString;
            }
        }
    }
}