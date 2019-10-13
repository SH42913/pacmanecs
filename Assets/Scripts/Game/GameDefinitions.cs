using Game.Ghosts;
using Game.Items.Food;
using Game.Players;
using Game.Portals;
using Game.World;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "PacManEcs/GameDefinitions", fileName = "GameDefs")]
    public class GameDefinitions : ScriptableObject
    {
        public WorldDefinition worldDefinition;
        public PlayerDefinition playerDefinition;
        public FoodDefinition foodDefinition;
        public GhostDefinition ghostDefinition;
        public PortalDefinition portalDefinition;
    }
}