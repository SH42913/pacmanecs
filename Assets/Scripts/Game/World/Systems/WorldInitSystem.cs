using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.World
{
    public class WorldInitSystem : IEcsPreInitSystem
    {
        private readonly WorldService _worldService = null;
        private readonly GameDefinitions _gameDefinitions = null;

        public void PreInit()
        {
            if (!_gameDefinitions.worldDefinition)
            {
                throw new Exception($"{nameof(WorldDefinition)} doesn't exists!");
            }

            WorldDefinition worldDefinition = _gameDefinitions.worldDefinition;

            _worldService.WorldField = new HashSet<EcsEntity>[worldDefinition.SizeX][];
            for (int xIndex = 0, xMax = worldDefinition.SizeX; xIndex < xMax; xIndex++)
            {
                var yFields = new HashSet<EcsEntity>[worldDefinition.SizeY];
                for (int yIndex = 0, yMax = worldDefinition.SizeY; yIndex < yMax; yIndex++)
                {
                    yFields[yIndex] = new HashSet<EcsEntity>();
                }

                _worldService.WorldField[xIndex] = yFields;
            }

#if DEBUG
            Vector3 finalX = Vector3.right * worldDefinition.SizeX;
            Vector3 finalY = Vector3.forward * worldDefinition.SizeY;
            Vector3 final = finalX + finalY;

            Debug.DrawLine(Vector3.zero, finalX, Color.yellow, 5000);
            Debug.DrawLine(Vector3.zero, finalY, Color.yellow, 5000);
            Debug.DrawLine(finalX, final, Color.yellow, 5000);
            Debug.DrawLine(finalY, final, Color.yellow, 5000);
#endif
        }
    }
}