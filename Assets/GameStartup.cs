using System;
using Game.Gameplay;
using Game.Gameplay.Ghosts;
using Game.Gameplay.Items;
using Game.Gameplay.Items.Food;
using Game.Gameplay.Moving;
using Game.Gameplay.Players;
using Game.Gameplay.Portals;
using Game.Gameplay.Teleports;
using Game.Gameplay.Walls;
using Game.Gameplay.World;
using Game.UI;
using Game.UI.GameStates;
using Game.UI.ScoreTable;
using Leopotam.Ecs;
using UnityEngine;

public sealed class GameStartup : MonoBehaviour {
    public GameObject pauseMenu;
    public GameDefinitions gameDefinitions;

    private EcsWorld ecsWorld;
    private EcsSystems systems;
    private System.Random random;

    private void OnEnable() {
        if (!gameDefinitions) throw new Exception($"{nameof(GameDefinitions)} doesn't exists!");

        ecsWorld = new EcsWorld();
        systems = new EcsSystems(ecsWorld);
        random = new System.Random();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(ecsWorld);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif

        systems.Add(new WorldInitSystem())
            .Add(new PlayerInitSystem())
            .Add(new GhostInitSystem())
            .Add(new WallInitSystem())
            .Add(new PortalInitSystem())
            .Add(new FoodInitSystem())
            .Add(new ScoreTableInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new GhostSystem())
            .Add(new MovementSystem())
            .Add(new ItemSystem())
            .Add(new FoodSystem())
            .Add(new EnergizerSystem())
            .OneFrame<ItemTakenEvent>()
            .Add(new GhostFearStateSystem())
            .OneFrame<GhostFearStateRequest>()
            .Add(new PlayerDeathSystem())
            .OneFrame<PlayerDeathRequest>()
            .Add(new PortalSystem())
            .Add(new TeleportSystem())
            .OneFrame<TeleportToPositionRequest>()
            .Add(new WorldSystem())
            .OneFrame<WorldObjectCreateRequest>()
            .OneFrame<WorldObjectDestroyedEvent>()
            .OneFrame<WorldObjectNewPositionRequest>()
            .AddUiSystems()
            .Inject(new WorldService())
            .Inject(gameDefinitions)
            .Inject(random)
            .ProcessInjects()
            .Init();

        ecsWorld.NewEntity().Get<PauseMenuComponent>().gameObject = pauseMenu;
        StartGame();
    }

    private void Update() {
        systems.Run();

        if (Input.GetKeyUp(KeyCode.Escape)) {
            ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = Time.timeScale < 1
                ? GameStates.Start
                : GameStates.Pause;
        }
    }

    private void OnDisable() {
        systems.Destroy();
        systems = null;

        ecsWorld.Destroy();
        ecsWorld = null;
    }

    private void StartGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Start;
    }

    public void RestartGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Restart;
    }

    public void ContinueGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Start;
    }

    public void QuitGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Exit;
    }
}