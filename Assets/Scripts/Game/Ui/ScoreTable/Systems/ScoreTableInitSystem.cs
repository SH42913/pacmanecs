using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.ScoreTable
{
    public class ScoreTableInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Init()
        {
            ScoreTableBehaviour[] scoreTables = Object.FindObjectsOfType<ScoreTableBehaviour>();
            foreach (ScoreTableBehaviour behaviour in scoreTables)
            {
                _ecsWorld.NewEntityWith(out ScoreTableComponent scoreTable);
                scoreTable.ScoreText = behaviour.GetComponent<Text>();
            }

            _ecsWorld.NewEntityWith(out UpdateScoreTableEvent _);
        }
    }
}