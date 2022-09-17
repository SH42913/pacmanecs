using System;
using Game.Gameplay;
using Game.Gameplay.Ghosts;
using Game.Gameplay.Items;
using Game.Gameplay.Items.Food;
using Game.Gameplay.Movement;
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
    public PauseMenuComponent pauseMenu;
    public WallRegistry wallRegistry;
    public GameDefinitions gameDefinitions;

    [Header("Food")] public Transform[] foodTransforms;
    public Transform[] energizerTransforms;

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
            .Add(new FoodInitSystem(foodTransforms, energizerTransforms))
            .Add(new ScoreTableInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new GhostSystem())
            .Add(new MovementTargetSystem())
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
            .OneFrame<PlayerScoreChangedEvent>()
            .Inject(new WorldService())
            .Inject(gameDefinitions)
            .Inject(wallRegistry)
            .Inject(random)
            .ProcessInjects();

        ecsWorld.NewEntity().Replace(pauseMenu);
        StartGame();
        systems.Init();
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

    private void StartGame() {
        ecsWorld.NewEntity().Get<GameStateSwitchRequest>().newState = GameStates.Start;
    }
}