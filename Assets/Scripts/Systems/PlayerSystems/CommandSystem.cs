using Components.BaseComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class CommandSystem : IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<CommandComponent> Commands { get; set; }
        private EcsFilter<MoveComponent, PlayerComponent> Players { get; set; }
        
        public void Run()
        {
            for (int i = 0; i < Commands.EntitiesCount; i++)
            {
                var command = Commands.Components1[i];

                var moveComponent = Players
                    .GetFirstComponent(null, x => x.Num == command.PlayerNum);
                if (moveComponent != null)
                {
                    moveComponent.Heading = command.NewDirection;
                }
                
                RemoveCommandsWhere(command.PlayerNum);
            }
        }

        private void RemoveCommandsWhere(int playerNum)
        {
            for (int i = 0; i < Commands.EntitiesCount; i++)
            {
                var command = Commands.Components1[i];
                if(command.PlayerNum != playerNum) continue;
                
                EcsWorld.RemoveEntity(Commands.Entities[i]);
            }
        }
    }
}