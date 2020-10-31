using Game.Gameplay.Players;
using Leopotam.Ecs;

namespace Game.UI.ScoreTable {
    public sealed class ScoreTableSystem : IEcsRunSystem {
        private readonly EcsFilter<PlayerComponent> players = null;
        private readonly EcsFilter<ScoreTableComponent> scoreTables = null;
        private readonly EcsFilter<PlayerScoreChangedEvent> updateEvents = null;
        private readonly EcsFilter<ScoreTableComponent>.Exclude<InitializedScoreTableMarker> tablesToInit = null;

        public void Run() {
            if (updateEvents.IsEmpty() && tablesToInit.IsEmpty()) return;

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
                scoreTables.GetEntity(i).Get<InitializedScoreTableMarker>();
            }
        }

        private struct InitializedScoreTableMarker { }
    }
}