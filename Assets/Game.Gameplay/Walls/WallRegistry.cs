using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Walls {
    public sealed class WallRegistry : MonoBehaviour {
        public Transform[] walls;

        [ContextMenu(nameof(FindAllWalls))]
        public void FindAllWalls() {
            walls = GameObject.FindGameObjectsWithTag("Wall").Select(x => x.transform).ToArray();
        }
    }
}