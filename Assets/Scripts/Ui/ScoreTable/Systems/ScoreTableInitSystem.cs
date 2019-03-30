using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.ScoreTable.Systems
{
    [EcsInject]
    public class ScoreTableInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Initialize()
        {
            ScoreTableBehaviour[] scoreTables = Object.FindObjectsOfType<ScoreTableBehaviour>();
            foreach (ScoreTableBehaviour behaviour in scoreTables)
            {
                _ecsWorld.CreateEntityWith(out ScoreTableComponent scoreTable);
                scoreTable.ScoreText = behaviour.GetComponent<Text>();
            }
            
            _ecsWorld.CreateEntityWith(out UpdateScoreTableEvent _);
        }

        public void Destroy()
        {
        }
    }
}