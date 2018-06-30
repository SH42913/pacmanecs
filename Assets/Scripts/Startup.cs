using Systems.BaseSystems;
using Systems.GhostSystems;
using Systems.ItemSystems;
using Systems.PlayerSystems;
using Systems.StaticSystems;
using Components.BaseComponents;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
	[Header("Gameplay elements"),
	 SerializeField, Range(0, 10)]
	private int _startLifes;

	[SerializeField, Range(0, 10)]
	private float _ghostSpeed;

	[SerializeField, Range(0, 10)]
	private float _startPacmanSpeed;
	
	[Header("Gui Elements"), SerializeField]
	private Text _scoresText;

	[SerializeField]
	private GameObject _pauseMenu;
	
	private EcsWorld EcsWorld { get; set; }
	private EcsSystems UpdateSystems { get; set; }
	
	// Use this for initialization
	private void Start () 
	{
		EcsWorld = new EcsWorld();
#if UNITY_EDITOR
		LeopotamGroup.Ecs.UnityIntegration.EcsWorldObserver.Create (EcsWorld);
#endif  
		UpdateSystems = new EcsSystems(EcsWorld)
			.Add(new PlayerInitSystem
			{
				StartLifes = _startLifes,
				StartSpeed = _startPacmanSpeed
			})
			.Add(new WallInitSystem())
			.Add(new PlayerInputSystem())
			.Add(new CommandSystem())
			.Add(new GhostSystem
			{
				GhostSpeed = _ghostSpeed
			})
			.Add(new MoveSystem())
			.Add(new PortalSystem())
			.Add(new DeathSystem())
			.Add(new TeleportSystem())
			.Add(new GuiSystem
			{
				Text = _scoresText
			})
			.Add(new GameStateSystem
			{
				GuiElement = _pauseMenu
			})
			.AddItemSystems();
		
		UpdateSystems.Initialize();
#if UNITY_EDITOR
		LeopotamGroup.Ecs.UnityIntegration.EcsSystemsObserver.Create (UpdateSystems);
#endif
		EcsWorld.CreateEntityWith<GameStateComponent>().State = GameStates.START;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		UpdateSystems.Run();	
	}

	private void OnDestroy()
	{
		UpdateSystems.Destroy();
	}

	public void RestartGame()
	{
		EcsWorld.CreateEntityWith<GameStateComponent>().State = GameStates.RESTART;
	}

	public void UnpauseGame()
	{
		EcsWorld.CreateEntityWith<GameStateComponent>().State = GameStates.START;
	}

	public void QuitGame()
	{
		EcsWorld.CreateEntityWith<GameStateComponent>().State = GameStates.EXIT;
	}
}

public static class SystemExtensions
{
	public static EcsSystems AddItemSystems(this EcsSystems systems)
	{
		systems
			.Add(new ItemSystem
			{
				FoodPenalty = 0.001f
			})
			.Add(new FoodSystem())
			.Add(new EnergizerSystem());

		return systems;
	}
}