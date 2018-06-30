using System.Collections.Generic;
using Components.BaseComponents;
using Components.StaticComponents;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.StaticSystems
{
    [EcsInject]
    public class PortalSystem : IEcsInitSystem, IEcsRunSystem
    {
        public float TimeToReloadPortals = 1f;
        
        private EcsWorld _ecsWorld = null;
        private EcsFilter<PositionComponent, PortalComponent> _portals = null;
        private EcsFilter<PositionComponent, MoveComponent> _moveables = null;

        public void Initialize()
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");

            foreach (GameObject portal in portals)
            {
                int? channelNum = GetChannelFrom(portal);
                if (!channelNum.HasValue)
                {
                    Debug.LogWarning(string.Format("Portal {0} has wrong name!", portal.name));
                    continue;
                }

                int entity = portal.CreateEntityWithPosition(_ecsWorld);
                _ecsWorld
                    .AddComponent<PortalComponent>(entity)
                    .Channel = channelNum.Value;
                
            }
        }

        public void Destroy()
        {
            
        }

        public void Run()
        {
            for (int i = 0; i < _portals.EntitiesCount; i++)
            {
                var portalPosition = _portals.Components1[i];
                var portalComponent = _portals.Components2[i];
                if (portalComponent.TimeToReload > 0)
                {
                    portalComponent.TimeToReload -= Time.deltaTime;
                    continue;
                }

                var objectToTeleport = _moveables.GetSecondComponent(x => x.Position == portalPosition.Position);
                if(objectToTeleport == null) continue;

                var portalList = FindPortalsWithChannel(_portals.Components2[i].Channel);
                portalList.Remove(portalPosition);
                if(portalList.Count == 0) continue;

                int selectedPortal = Random.Range(0, portalList.Count);
                var otherPortalPosition = portalList[selectedPortal].Position;
                var teleportComponent = _ecsWorld.CreateEntityWith<TeleportComponent>();
                teleportComponent.MoveComponent = objectToTeleport;
                teleportComponent.TargetPosition = otherPortalPosition.ToVector3(objectToTeleport.Transform.position.y);

                portalComponent.TimeToReload = TimeToReloadPortals;
                var otherPortalEntity = GetIndexOf(portalList[selectedPortal]);
                if(!otherPortalEntity.HasValue) continue;
                
                _portals.Components2[otherPortalEntity.Value].TimeToReload = TimeToReloadPortals;
            }
        }

        private int? GetChannelFrom(GameObject portal)
        {
            int colonPosition = portal.name.IndexOf(':');
            if (colonPosition < 0) return null;

            string channelString = portal.name.Substring(colonPosition + 1, 1);
            int channelNum;

            return int.TryParse(channelString, out channelNum)
                ? (int?) channelNum
                : null;
        }

        private List<PositionComponent> FindPortalsWithChannel(int channelNum)
        {
            var list = new List<PositionComponent>();
            for (int i = 0; i < _portals.EntitiesCount; i++)
            {
                if (_portals.Components2[i].Channel != channelNum) continue;
                list.Add(_portals.Components1[i]);
            }
            return list;
        }

        private int? GetIndexOf(PositionComponent component)
        {
            for (int i = 0; i < _portals.EntitiesCount; i++)
            {
                if(!_portals.Components1[i].Equals(component)) continue;
                return i;
            }

            return null;
        }
    }
}