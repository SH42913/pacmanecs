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
            var scoreTables = Object.FindObjectsOfType<ScoreTableBehaviour>();
            foreach (ScoreTableBehaviour behaviour in scoreTables)
            {
                _ecsWorld.CreateEntityWith<ScoreTableComponent>().ScoreText = behaviour.GetComponent<Text>();
            }
            
            _ecsWorld.CreateEntityWith<UpdateScoreTableEvent>();
        }

        public void Destroy()
        {
        }
    }
}