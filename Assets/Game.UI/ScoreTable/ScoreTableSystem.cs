using System.Text;
using Game.Gameplay.Players;
using Leopotam.Ecs;

namespace Game.UI.ScoreTable {
    public sealed class ScoreTableSystem : IEcsRunSystem {
        private readonly EcsFilter<PlayerComponent> players = null;
        private readonly EcsFilter<ScoreTableComponent> scoreTables = null;
        private readonly EcsFilter<PlayerScoreChangedEvent> updateEvents = null;
        private readonly EcsFilter<ScoreTableComponent>.Exclude<InitializedScoreTableMarker> tablesToInit = null;

        private readonly StringBuilder stringBuilder = new StringBuilder("P10 Scores:100000 Lives:100");

        public void Run() {
            if (updateEvents.IsEmpty() && tablesToInit.IsEmpty()) return;

            stringBuilder.Clear();

            foreach (var i in players) {
                ref var player = ref players.Get1(i);

                stringBuilder.Append("P");
                stringBuilder.Append(player.num);

                if (players.GetEntity(i).Has<DeadPlayerMarker>()) {
                    stringBuilder.Append(" IS DEAD");
                } else {
                    stringBuilder.Append(" Lives:");
                    stringBuilder.Append(player.lives);
                }

                stringBuilder.Append(" Scores:");
                stringBuilder.Append(player.scores);
                stringBuilder.AppendLine();
            }

            foreach (var i in scoreTables) {
                scoreTables.Get1(i).scoreText.text = stringBuilder.ToString();
                scoreTables.GetEntity(i).Get<InitializedScoreTableMarker>();
            }
        }

        private struct InitializedScoreTableMarker { }
    }
}