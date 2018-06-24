using Systems;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
	[SerializeField]
	private Text _scoresText;

	[SerializeField]
	private uint _startLifes;

	[SerializeField]
	private float _ghostSpeed;
	
	private EcsWorld EcsWorld { get; set; }
	private EcsSystems UpdateSystems { get; set; }
	
	// Use this for initialization
	private void Start () 
	{
		EcsWorld = new EcsWorld();
		UpdateSystems = new EcsSystems(EcsWorld)
			.Add(new PlayerSystem
			{
				StartLifes = _startLifes
			})
			.Add(new WallInitSystem())
			.Add(new PlayerInputSystem())
			.Add(new PlayerInputSystem
			{
				PlayerNum = 2,
				VerticalAxisName = "PlayerTwoY",
				HorizontAxisName = "PlayerTwoX"
			})
			.Add(new CommandSystem())
			.Add(new GhostSystem
			{
				GhostSpeed = _ghostSpeed
			})
			.Add(new PortalSystem())
			.Add(new MoveSystem())
			.Add(new DeathSystem())
			.Add(new ItemSystem())
			.Add(new GuiSystem
			{
				Text = _scoresText
			});
		
		UpdateSystems.Initialize();
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
}
