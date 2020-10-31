using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Ui.ScoreTable {
    public sealed class ScoreTableInitSystem : IEcsInitSystem {
        private readonly EcsWorld ecsWorld = null;

        public void Init() {
            var scoreTables = Object.FindObjectsOfType<ScoreTableBehaviour>();
            foreach (var behaviour in scoreTables) {
                ecsWorld.NewEntity().Get<ScoreTableComponent>().scoreText = behaviour.GetComponent<Text>();
            }

            ecsWorld.NewEntity().Get<ScoreTableNeedUpdateEvent>();
        }
    }
}