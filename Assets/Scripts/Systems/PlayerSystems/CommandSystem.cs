using Components.BaseComponents;
using Components.PlayerComponents;
using LeopotamGroup.Ecs;

namespace Systems.PlayerSystems
{
    [EcsInject]
    public class CommandSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld = null;
        private EcsFilter<CommandComponent> _commands = null;
        private EcsFilter<MoveComponent, PlayerComponent> _players = null;
        
        public void Run()
        {
            for (int i = 0; i < _commands.EntitiesCount; i++)
            {
                var command = _commands.Components1[i];

                var moveComponent = _players
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
            for (int i = 0; i < _commands.EntitiesCount; i++)
            {
                var command = _commands.Components1[i];
                if(command.PlayerNum != playerNum) continue;
                
                _ecsWorld.RemoveEntity(_commands.Entities[i]);
            }
        }
    }
}