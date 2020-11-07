using System.Linq;
using Game.Gameplay.Walls;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Movement {
    public sealed class MovementTargetSystem : IEcsRunSystem {
        private readonly WorldService worldService = null;
        private readonly EcsFilter<PositionComponent, MovementComponent, MovementTargetComponent, MovementStoppedMarker> entities = null;

        public void Run() {
            foreach (var i in entities) {
                ref var movement = ref entities.Get2(i);
                var currentPosition = entities.Get1(i).position;
                var targetPosition = entities.Get3(i).target;

                Directions? newHeading = null;
                Directions? alternateHeading = null;
                var sqrMagnitude = (targetPosition - currentPosition).sqrMagnitude;
                foreach (var direction in DirectionUtils.availableDirections) {
                    var possiblePosition = direction.GetPosition(currentPosition);
                    if (WallOnPosition(possiblePosition)) continue;

                    var possibleSqrMagnitude = (targetPosition - possiblePosition).sqrMagnitude;
                    if (possibleSqrMagnitude <= sqrMagnitude) {
                        newHeading = direction;
                        sqrMagnitude = possibleSqrMagnitude;
                    } else {
                        alternateHeading = direction;
                    }
                }

                movement.heading = newHeading ?? alternateHeading ?? movement.heading;
            }
        }

        private bool WallOnPosition(in Vector2Int position) {
            return worldService.GetEntitiesOn(position).Any(entity => entity.IsAlive() && entity.Has<WallMarker>());
        }
    }
}