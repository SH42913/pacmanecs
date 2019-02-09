using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Object = UnityEngine.Object;

namespace World.Systems
{
    [EcsInject]
    public class WorldInitSystem : IEcsPreInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void PreInitialize()
        {
            var worldConfig = Object.FindObjectOfType<WorldConfigBehaviour>();
            if (worldConfig == null)
            {
                throw new Exception("WorldConfigBehaviour must be created!");
            }

            var world = _ecsWorld.CreateEntityWith<WorldComponent>();
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
        }

        public void PreDestroy()
        {
        }
    }
}