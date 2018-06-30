using Components.BaseComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;
using UnityEngine.UI;

namespace Systems.BaseSystems
{
    [EcsInject]
    public class GuiSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<UpdateGuiComponent> _updateComponents = null;

        public Text Text;
        
        public void Run()
        {
            if(_updateComponents.EntitiesCount == 0) return;
            
            var scoreText = "";
            for (int i = 0; i < _players.EntitiesCount; i++)
            {
                var player = _players.Components1[i];

                scoreText += string.Format("P{0} Scores:{1}", player.Num, player.Scores);

                if (!player.IsDead)
                {
                    scoreText += string.Format("Lifes:{0}\n", player.Lifes);
                }
                else
                {
                    scoreText += "IS DEAD\n";
                }
            }
            Text.text = scoreText;

            for (int i = 0; i < _updateComponents.EntitiesCount; i++)
            {
                _ecsWorld.RemoveEntity(_updateComponents.Entities[i]);
            }
        }
    }
}