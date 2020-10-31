using System;
using Game.Gameplay;
using Game.Gameplay.Ghosts;
using Game.Gameplay.Items;
using Game.Gameplay.Items.Food;
using Game.Gameplay.Moving;
using Game.Gameplay.Players;
using Game.Gameplay.Portals;
using Game.Gameplay.Teleports;
using Game.Gameplay.Ui.GameStates;
using Game.Gameplay.Ui.ScoreTable;
using Game.Gameplay.Walls;
using Game.Gameplay.World;
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
            .Add(new GhostFearStateSystem())
            .OneFrame<GhostFearStateRequest>()
            .Add(new PlayerDeathSystem())
            .OneFrame<PlayerDeathRequest>()
            .Add(new PortalSystem())
            .Add(new TeleportSystem())
            .Add(new ScoreTableSystem())
            .Add(new GameStateSystem())
            .OneFrame<GameStateSwitchRequest>()
            .OneFrame<ScoreTableNeedUpdateEvent>()
            .Add(new WorldSystem())
            .OneFrame<TeleportToPositionRequest>()
            .OneFrame<ItemTakenEvent>()
            .OneFrame<WorldObjectCreateRequest>()
            .OneFrame<WorldObjectDestroyedEvent>()
            .OneFrame<WorldObjectNewPositionRequest>()
            .Inject(new WorldService())
            .Inject(gameDefinitions)
            .Inject(random)
            .ProcessInjects()
            .Init();

        InitPauseMenu();
        StartGame();
    }

    private void Update() {
        systems.Run();
    }

    private void OnDisable() {
        systems.Destroy();
        systems = null;

        ecsWorld.Destroy();
        ecsWorld = null;
    }

    private void InitPauseMenu() {
        ecsWorld.NewEntity().Get<PauseMenuComponent>().gameObject = pauseMenu;
    }

    private void StartGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().state = GameStates.Start;
    }

    public void RestartGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().state = GameStates.Restart;
    }

    public void ContinueGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().state = GameStates.Start;
    }

    public void QuitGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().state = GameStates.Exit;
    }
}