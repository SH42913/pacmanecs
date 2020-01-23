using System;
using Game.Death;
using Game.Ghosts;
using Game.Items.Food;
using Game.Items;
using Game.Moving;
using Game.Players;
using Game.Portals;
using Game.Teleports;
using Game.Ui.GameStates;
using Game.Ui.ScoreTable;
using Game.Walls;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public sealed class GameStartup : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameDefinitions gameDefinitions;

        private EcsWorld _ecsWorld;
        private EcsSystems _systems;
        private System.Random _random;

        private void OnEnable()
        {
            if (!gameDefinitions)
            {
                throw new Exception($"{nameof(GameDefinitions)} doesn't exists!");
            }

            _ecsWorld = new EcsWorld();
            _systems = new EcsSystems(_ecsWorld);
            _random = new System.Random();

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_ecsWorld);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif

            _systems
                .Add(new WorldInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new GhostInitSystem())
                .Add(new WallInitSystem())
                .Add(new PortalInitSystem())
                .Add(new FoodInitSystem())
                .Add(new ScoreTableInitSystem())
                .Add(new PlayerInputSystem())
                .Add(new GhostSystem())
                .Add(new UpdateDirectionSystem())
                .Add(new MoveSystem())
                .Add(new ItemSystem())
                .Add(new FoodSystem())
                .Add(new EnergizerSystem())
                .Add(new GhostFearStateSystem())
                .Add(new DeathSystem())
                .Add(new PortalSystem())
                .Add(new TeleportSystem())
                .Add(new WorldSystem())
                .Add(new ScoreTableSystem())
                .Add(new GameStateSystem())
                .Inject(new WorldService())
                .Inject(gameDefinitions)
                .Inject(_random)
                .ProcessInjects()
                .Init();

            InitPauseMenu();
            StartGame();
        }

        private void Update()
        {
            _systems.Run();
            _ecsWorld.EndFrame();
        }

        private void OnDisable()
        {
            _systems.Destroy();
            _systems = null;

            _ecsWorld.Destroy();
            _ecsWorld = null;
        }

        private void InitPauseMenu()
        {
            _ecsWorld.NewEntityWith(out PauseMenuComponent pauseMenu);
            pauseMenu.GameObject = PauseMenu;
        }

        private void StartGame()
        {
            _ecsWorld.NewEntityWith(out ChangeGameStateEvent changeGameStateEvent);
            changeGameStateEvent.State = GameStates.Start;
        }

        public void RestartGame()
        {
            _ecsWorld.NewEntityWith(out ChangeGameStateEvent changeGameStateEvent);
            changeGameStateEvent.State = GameStates.Restart;
        }

        public void ContinueGame()
        {
            _ecsWorld.NewEntityWith(out ChangeGameStateEvent changeGameStateEvent);
            changeGameStateEvent.State = GameStates.Start;
        }

        public void QuitGame()
        {
            _ecsWorld.NewEntityWith(out ChangeGameStateEvent changeGameStateEvent);
            changeGameStateEvent.State = GameStates.Exit;
        }
    }
}