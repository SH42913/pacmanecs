using Leopotam.Ecs;
using Players;
using UnityEngine.UI;

namespace Ui.ScoreTable.Systems
{
    [EcsInject]
    public class ScoreTableSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent> _players = null;
        private readonly EcsFilter<ScoreTableComponent> _scoreTables = null;
        private readonly EcsFilter<UpdateScoreTableEvent> _updateEvents = null;

        public void Run()
        {
            if (_updateEvents.EntitiesCount <= 0) return;

            string scoresString = "";
            for (int i = 0; i < _players.EntitiesCount; i++)
            {
                PlayerComponent player = _players.Components1[i];
                scoresString += $"P{player.Num} Scores:{player.Scores} ";

                if (player.IsDead)
                {
                    scoresString += "IS DEAD\n";
                }
                else
                {
                    scoresString += $"Lives:{player.Lives}\n";
                }
            }

            foreach (int i in _scoreTables)
            {
                Text unityText = _scoreTables.Components1[i].ScoreText;
                unityText.text = scoresString;
            }
        }
    }
}