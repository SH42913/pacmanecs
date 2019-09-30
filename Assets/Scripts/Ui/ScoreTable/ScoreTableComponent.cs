using Leopotam.Ecs;
using UnityEngine.UI;

namespace Ui.ScoreTable
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