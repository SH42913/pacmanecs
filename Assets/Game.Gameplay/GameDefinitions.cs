using Game.Gameplay.Ghosts;
using Game.Gameplay.Items.Food;
using Game.Gameplay.Players;
using Game.Gameplay.Portals;
using Game.Gameplay.World;
using UnityEngine;

namespace Game.Gameplay {
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(GameDefinitions))]
    public sealed class GameDefinitions : ScriptableObject {
        public WorldDefinition worldDefinition;
        public PlayerDefinition playerDefinition;
        public FoodDefinition foodDefinition;
        public GhostDefinition ghostDefinition;
        public PortalDefinition portalDefinition;
    }
}