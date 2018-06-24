using System;
using UnityEngine;

namespace Components
{
    public enum ItemTypes
    {
        Food,
        Energizer
    }
    
    public class ItemComponent
    {
        public GameObject GameObject { get; set; }
        public ItemTypes ItemType { get; set; }
        public Action<MoveComponent, PlayerComponent> UseAction { get; set; }
        public bool Used { get; set; }
    }
}