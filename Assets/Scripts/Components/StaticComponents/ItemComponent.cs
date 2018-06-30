using System;
using Components.BaseComponents;
using Components.PlayerComponents;
using UnityEngine;

namespace Components.StaticComponents
{
    public enum ItemTypes
    {
        Food,
        Energizer
    }
    
    public class ItemComponent
    {
        public int ItemEntity;
        public GameObject GameObject;
        public ItemTypes ItemType;
        public Action<MoveComponent, PlayerComponent> UseAction;
        public bool Used;
    }
}