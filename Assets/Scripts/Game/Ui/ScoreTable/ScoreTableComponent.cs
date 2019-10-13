using Leopotam.Ecs;
using UnityEngine.UI;

namespace Game.Ui.ScoreTable
{
    public class ScoreTableComponent : IEcsAutoReset
    {
        public Text ScoreText;

        public void Reset()
        {
            ScoreText = null;
        }
    }
}