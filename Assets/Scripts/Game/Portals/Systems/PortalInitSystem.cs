using System.Collections.Generic;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Portals {
    public class PortalInitSystem : IEcsInitSystem {
        private readonly EcsWorld _ecsWorld = null;

        public void Init() {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            var channelDict = new Dictionary<int, EcsEntity>();
            var filledChannels = new HashSet<int>();

            foreach (GameObject portal in portals) {
                int? channelNum = GetChannelFrom(portal);
                if (!channelNum.HasValue) {
                    Debug.LogError($"Portal {portal.name} has wrong name!");
                    continue;
                }

                int channel = channelNum.Value;
                if (filledChannels.Contains(channel)) {
                    Debug.LogError($"Channel {channel.ToString()} for portal {portal.name} already used!");
                    continue;
                }

                EcsEntity portalEntity = _ecsWorld.NewEntity();
                portalEntity.Get<CreateWorldObjectEvent>().Transform = portal.transform;
                ref var portalComponent = ref portalEntity.Get<PortalComponent>();

                if (channelDict.ContainsKey(channel)) {
                    filledChannels.Add(channel);

                    EcsEntity otherPortalEntity = channelDict[channel];
                    portalComponent.OtherPortalEntity = otherPortalEntity;
                    otherPortalEntity.Get<PortalComponent>().OtherPortalEntity = portalEntity;
                } else {
                    channelDict.Add(channel, portalEntity);
                }
            }
        }

        private static int? GetChannelFrom(Object portal) {
            int colonPosition = portal.name.IndexOf(':');
            if (colonPosition < 0) return null;

            string channelString = portal.name.Substring(colonPosition + 1, 1);
            return int.TryParse(channelString, out int channelNum)
                ? (int?) channelNum
                : null;
        }
    }
}