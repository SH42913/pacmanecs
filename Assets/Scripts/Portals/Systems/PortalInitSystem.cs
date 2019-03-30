using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using World;

namespace Portals.Systems
{
    [EcsInject]
    public class PortalInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Initialize()
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            var channelDict = new Dictionary<int, EcsEntity>();
            var filledChannels = new HashSet<int>();

            foreach (GameObject portal in portals)
            {
                int? channelNum = GetChannelFrom(portal);
                if (!channelNum.HasValue)
                {
                    Debug.LogError($"Portal {portal.name} has wrong name!");
                    continue;
                }

                int channel = channelNum.Value;
                if (filledChannels.Contains(channel))
                {
                    Debug.LogError($"Channel {channel} for portal {portal.name} already used!");
                    continue;
                }

                EcsEntity portalEntity = _ecsWorld.CreateEntityWith(out PortalComponent portalComponent);
                _ecsWorld.AddComponent<CreateWorldObjectEvent>(portalEntity).Transform = portal.transform;

                if (channelDict.ContainsKey(channel))
                {
                    filledChannels.Add(channel);
                    EcsEntity otherPortalEntity = channelDict[channel];
                    portalComponent.OtherPortalEntity = channelDict[channel];
                    _ecsWorld
                        .GetComponent<PortalComponent>(otherPortalEntity)
                        .OtherPortalEntity = portalEntity;
                }
                else
                {
                    channelDict.Add(channel, portalEntity);
                }
            }
        }

        public void Destroy()
        {
        }

        private static int? GetChannelFrom(Object portal)
        {
            int colonPosition = portal.name.IndexOf(':');
            if (colonPosition < 0) return null;

            string channelString = portal.name.Substring(colonPosition + 1, 1);
            return int.TryParse(channelString, out int channelNum)
                ? (int?) channelNum
                : null;
        }
    }
}