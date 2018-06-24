using Components;
using LeopotamGroup.Ecs;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class GuiSystem : IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<PlayerComponent> Players { get; set; }
        private EcsFilter<UpdateGuiComponent> UpdateComponents { get; set; }

        public Text Text { get; set; }
        
        public void Run()
        {
            if(UpdateComponents.EntitiesCount == 0) return;
            
            var scoreText = "";
            for (int i = 0; i < Players.EntitiesCount; i++)
            {
                var player = Players.Components1[i];
                
                scoreText += $"P{player.Num} " +
                             $"Scores:{player.Scores} ";

                if (!player.IsDead)
                {
                    scoreText += $"Lifes:{player.Lifes}\n";
                }
                else
                {
                    scoreText += $"IS DEAD\n";
                }
            }
            Text.text = scoreText;

            for (int i = 0; i < UpdateComponents.EntitiesCount; i++)
            {
                EcsWorld.RemoveEntity(UpdateComponents.Entities[i]);
            }
        }
    }
}