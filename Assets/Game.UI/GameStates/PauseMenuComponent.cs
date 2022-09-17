using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.GameStates {
    [Serializable]
    public struct PauseMenuComponent {
        public GameObject root;
        public Button continueBtn;
        public Button restartBtn;
        public Button quitBtn;
    }
}