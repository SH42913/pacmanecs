using Leopotam.Ecs;
using UnityEngine.UI;

namespace Ui.ScoreTable
{
    public class ScoreTableComponent : IEcsAutoResetComponent
    {
        public Text ScoreText;

        public void Reset()
        {
            ScoreText = null;
        }
    }
}