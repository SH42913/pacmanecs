using UnityEngine;

namespace Components.ItemComponents
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
    }
}