using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.World {
    public sealed class WorldInitSystem : IEcsPreInitSystem {
        private readonly WorldService worldService = null;
        private readonly GameDefinitions gameDefinitions = null;

        public void PreInit() {
            if (!gameDefinitions.worldDefinition) throw new Exception($"{nameof(WorldDefinition)} doesn't exists!");

            var worldDefinition = gameDefinitions.worldDefinition;
            worldService.worldField = new HashSet<EcsEntity>[worldDefinition.sizeX][];
            for (int xIndex = 0, xMax = worldDefinition.sizeX; xIndex < xMax; xIndex++) {
                var yFields = new HashSet<EcsEntity>[worldDefinition.sizeY];
                for (int yIndex = 0, yMax = worldDefinition.sizeY; yIndex < yMax; yIndex++) {
                    yFields[yIndex] = new HashSet<EcsEntity>();
                }

                worldService.worldField[xIndex] = yFields;
            }

#if DEBUG
            var finalX = Vector3.right * worldDefinition.sizeX;
            var finalY = Vector3.forward * worldDefinition.sizeY;
            var final = finalX + finalY;

            Debug.DrawLine(Vector3.zero, finalX, Color.yellow, 5000);
            Debug.DrawLine(Vector3.zero, finalY, Color.yellow, 5000);
            Debug.DrawLine(finalX, final, Color.yellow, 5000);
            Debug.DrawLine(finalY, final, Color.yellow, 5000);
#endif
        }
    }
}