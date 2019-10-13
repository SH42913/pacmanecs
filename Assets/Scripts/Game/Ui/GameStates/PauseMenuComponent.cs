using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ui.GameStates
{
    public class PauseMenuComponent : IEcsAutoReset
    {
        public GameObject GameObject;

        public void Reset()
        {
            GameObject = null;
        }
    }
}