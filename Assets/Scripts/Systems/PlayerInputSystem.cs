using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class PlayerInputSystem : IEcsRunSystem
    {
        public uint PlayerNum { get; set; } = 1;
        public string VerticalAxisName { get; set; } = "PlayerOneY";
        public string HorizontAxisName { get; set; } = "PlayerOneX";
        
        private EcsWorld EcsWorld { get; set; }
        
        public void Run()
        {
            var yAxis = Input.GetAxis(VerticalAxisName);
            var xAxis = Input.GetAxis(HorizontAxisName);

            if (yAxis > 0)
            {
                SendCommand(Directions.UP);
            }
            else if (yAxis < 0)
            {
                SendCommand(Directions.DOWN);
            }
            else if (xAxis > 0)
            {
                SendCommand(Directions.RIGHT);
            }
            else if(xAxis < 0)
            {
                SendCommand(Directions.LEFT);
            }
        }

        private void SendCommand(Directions newDirection)
        {
            var command = EcsWorld.CreateEntityWith<CommandComponent>();
            command.PlayerNum = PlayerNum;
            command.NewDirection = newDirection;
        }
    }
}