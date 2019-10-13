using System.Collections.Generic;
using Game.World;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Portals.Systems
{
    public class PortalInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        public void Init()
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
                    Debug.LogError($"Channel {channel.ToString()} for portal {portal.name} already used!");
                    continue;
                }

                EcsEntity portalEntity = _ecsWorld.NewEntityWith(
                    out PortalComponent portalComponent,
                    out CreateWorldObjectEvent createEvt);
                createEvt.Transform = portal.transform;

                if (channelDict.ContainsKey(channel))
                {
                    filledChannels.Add(channel);
                    EcsEntity otherPortalEntity = channelDict[channel];
                    portalComponent.OtherPortalEntity = channelDict[channel];
                    
                    PortalComponent otherPortal = otherPortalEntity.Get<PortalComponent>();
                    otherPortal.OtherPortalEntity = portalEntity;
                }
                else
                {
                    channelDict.Add(channel, portalEntity);
                }
            }
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