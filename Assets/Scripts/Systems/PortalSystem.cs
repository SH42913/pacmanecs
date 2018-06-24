using System.Collections.Generic;
using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
    [EcsInject]
    public class PortalSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld EcsWorld { get; set; }
        private EcsFilter<PositionComponent, PortalComponent> Portals { get; set; }
        private EcsFilter<PositionComponent, MoveComponent> Moveables { get; set; }

        public float TimeToReloadPortals { get; set; } = 1f;
        
        public void Initialize()
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");

            foreach (GameObject portal in portals)
            {
                int? channelNum = GetChannelFrom(portal);
                if (!channelNum.HasValue)
                {
                    Debug.LogWarning($"Portal {portal.name} has wrong name!");
                    continue;
                }

                int entity = portal.CreateEntityWithPosition(EcsWorld);
                EcsWorld
                    .AddComponent<PortalComponent>(entity)
                    .Channel = channelNum.Value;
                
            }
        }

        public void Destroy()
        {
            
        }

        public void Run()
        {
            for (int i = 0; i < Portals.EntitiesCount; i++)
            {
                var portalPosition = Portals.Components1[i];
                var portalComponent = Portals.Components2[i];
                if (portalComponent.TimeToReload > 0)
                {
                    portalComponent.TimeToReload -= Time.deltaTime;
                    continue;
                }

                var objectToTeleport = Moveables.GetSecondComponent(x => x.Position == portalPosition.Position);
                if(objectToTeleport == null) continue;

                var positionToTeleport = Moveables.GetFirstComponent(x => x.Position == portalPosition.Position);
                var portalList = FindPortalsWithChannel(Portals.Components2[i].Channel);
                portalList.Remove(portalPosition);
                if(portalList.Count == 0) continue;

                int selectedPortal = Random.Range(0, portalList.Count);
                var otherPortalPosition = portalList[selectedPortal].Position;
                var newPosition = otherPortalPosition.ToVector3(objectToTeleport.Transform.position.y);
                
                positionToTeleport.Position = otherPortalPosition;
                objectToTeleport.Transform.position = newPosition;
                objectToTeleport.DesiredPosition = newPosition;

                portalComponent.TimeToReload = TimeToReloadPortals;
                var otherPortalEntity = GetIndexOf(portalList[selectedPortal]);
                if(!otherPortalEntity.HasValue) continue;
                Portals.Components2[otherPortalEntity.Value].TimeToReload = TimeToReloadPortals;
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
            for (int i = 0; i < Portals.EntitiesCount; i++)
            {
                if (Portals.Components2[i].Channel != channelNum) continue;
                list.Add(Portals.Components1[i]);
            }
            return list;
        }

        private int? GetIndexOf(PositionComponent component)
        {
            for (int i = 0; i < Portals.EntitiesCount; i++)
            {
                if(!Portals.Components1[i].Equals(component)) continue;
                return i;
            }

            return null;
        }
    }
}