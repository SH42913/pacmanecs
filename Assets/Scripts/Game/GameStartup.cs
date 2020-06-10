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
using UnityEngine.Serialization;

namespace Game {
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
                .Add(new UpdateDirectionSystem())
                .Add(new MoveSystem())
                .Add(new ItemSystem())
                .Add(new FoodSystem())
                .Add(new EnergizerSystem())
                .Add(new GhostFearStateSystem())
                .OneFrame<EnableGhostFearStateEvent>()
                .Add(new DeathSystem())
                .OneFrame<PlayerIsDeadEvent>()
                .Add(new PortalSystem())
                .Add(new TeleportSystem())
                .Add(new ScoreTableSystem())
                .Add(new GameStateSystem())
                .OneFrame<ChangeGameStateEvent>()
                .OneFrame<UpdateScoreTableEvent>()
                .Add(new WorldSystem())
                .OneFrame<TeleportedEvent>()
                .OneFrame<TakenItemEvent>()
                .OneFrame<ChangeDirectionEvent>()
                .OneFrame<CreateWorldObjectEvent>()
                .OneFrame<DestroyedWorldObjectEvent>()
                .OneFrame<NewPositionEvent>()
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
            ecsWorld.NewEntity().Get<ChangeGameStateEvent>().state = GameStates.Start;
        }

        public void RestartGame() {
            ecsWorld.NewEntity().Get<ChangeGameStateEvent>().state = GameStates.Restart;
        }

        public void ContinueGame() {
            ecsWorld.NewEntity().Get<ChangeGameStateEvent>().state = GameStates.Start;
        }

        public void QuitGame() {
            ecsWorld.NewEntity().Get<ChangeGameStateEvent>().state = GameStates.Exit;
        }
    }
}