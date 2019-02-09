using Leopotam.Ecs;
using UnityEngine;

namespace Ui.GameStates
{
    public class PauseMenuComponent : IEcsAutoResetComponent
    {
        public GameObject GameObject;

        public void Reset()
        {
            GameObject = null;
        }
    }
}