using Ghosts;
using Items.Food;
using Players;
using UnityEngine;
using World;

[CreateAssetMenu(menuName = "PacManEcs/GameDefinitions", fileName = "GameDefs")]
public class GameDefinitions : ScriptableObject
{
    public WorldDefinition worldDefinition;
    public PlayerDefinition playerDefinition;
    public FoodDefinition foodDefinition;
    public GhostDefinition ghostDefinition;
}