using Ghosts;
using Items.Food;
using Players;
using UnityEngine;
using World;

[CreateAssetMenu(menuName = "PacManEcs/MainGameConfig")]
public class MainGameConfig : ScriptableObject
{
    public WorldConfig WorldConfig;
    public PlayerConfig PlayerConfig;
    public FoodConfig FoodConfig;
    public GhostConfig GhostConfig;
}