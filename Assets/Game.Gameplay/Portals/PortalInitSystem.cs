using System.Collections.Generic;
using Game.Gameplay.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Gameplay.Portals {
    public sealed class PortalInitSystem : IEcsInitSystem {
        private readonly EcsWorld ecsWorld = null;

        public void Init() {
            var portals = GameObject.FindGameObjectsWithTag("Portal");
            var channelDict = new Dictionary<int, EcsEntity>();
            var filledChannels = new HashSet<int>();

            foreach (var portal in portals) {
                var channelNum = GetChannelFrom(portal);
                if (!channelNum.HasValue) {
                    Debug.LogError($"Portal {portal.name} has wrong name!");
                    continue;
                }

                var channel = channelNum.Value;
                if (filledChannels.Contains(channel)) {
                    Debug.LogError($"Channel {channel.ToString()} for portal {portal.name} already used!");
                    continue;
                }

                var portalEntity = ecsWorld.NewEntity();
                portalEntity.Get<WorldObjectCreateRequest>().transform = portal.transform;
                ref var portalComponent = ref portalEntity.Get<PortalComponent>();

                if (channelDict.ContainsKey(channel)) {
                    filledChannels.Add(channel);

                    var otherPortalEntity = channelDict[channel];
                    portalComponent.otherPortalEntity = otherPortalEntity;
                    otherPortalEntity.Get<PortalComponent>().otherPortalEntity = portalEntity;
                } else {
                    channelDict.Add(channel, portalEntity);
                }
            }
        }

        private static int? GetChannelFrom(Object portal) {
            var colonPosition = portal.name.IndexOf(':');
            if (colonPosition < 0) return null;

            var channelString = portal.name.Substring(colonPosition + 1, 1);
            return int.TryParse(channelString, out var channelNum)
                ? (int?) channelNum
                : null;
        }
    }
}