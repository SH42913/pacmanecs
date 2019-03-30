using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace World.Systems
{
    [EcsInject]
    public class WorldInitSystem : IEcsPreInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private readonly MainGameConfig _mainGameConfig = null;

        public void PreInitialize()
        {
            if (!_mainGameConfig.WorldConfig)
            {
                throw new Exception($"{nameof(WorldConfig)} doesn't exists!");
            }

            var world = _ecsWorld.CreateEntityWith<WorldComponent>();
            WorldConfig worldConfig = _mainGameConfig.WorldConfig;
            
            world.WorldField = new HashSet<int>[worldConfig.SizeX][];
            for (int xIndex = 0, xMax = worldConfig.SizeX; xIndex < xMax; xIndex++)
            {
                var yFields = new HashSet<int>[worldConfig.SizeY];
                for (int yIndex = 0, yMax = worldConfig.SizeY; yIndex < yMax; yIndex++)
                {
                    yFields[yIndex] = new HashSet<int>();
                }

                world.WorldField[xIndex] = yFields;
            }

#if DEBUG
            Vector3 finalX = Vector3.right * worldConfig.SizeX;
            Vector3 finalY = Vector3.forward * worldConfig.SizeY;
            Vector3 final = finalX + finalY;
            
            Debug.DrawLine(Vector3.zero, finalX, Color.yellow, 5000);
            Debug.DrawLine(Vector3.zero, finalY, Color.yellow, 5000);
            Debug.DrawLine(finalX, final, Color.yellow, 5000);
            Debug.DrawLine(finalY, final, Color.yellow, 5000);
#endif
        }

        public void PreDestroy()
        {
        }
    }
}