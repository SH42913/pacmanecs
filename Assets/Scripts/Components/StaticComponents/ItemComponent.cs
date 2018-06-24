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
        public int ItemEntity { get; set; }
        public GameObject GameObject { get; set; }
        public ItemTypes ItemType { get; set; }
        public Action<MoveComponent, PlayerComponent> UseAction { get; set; }
        public bool Used { get; set; }
    }
}