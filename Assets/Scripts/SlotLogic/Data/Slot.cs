using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SlotLogic.Data
{
    [CreateAssetMenu]
    public class Slot : ScriptableObject
    {
        [SerializeField] private List<EquippedItem> equippedItems;
        
        [field: SerializeField] public int Size { get; private set; } = 6;

        public event Action<Dictionary<int, EquippedItem>> OnSlotUpdated;
        
        public void Initialize()
        {
            equippedItems = new List<EquippedItem>();
            for (int i = 0; i < Size; i++)
            {
                equippedItems.Add(EquippedItem.GetEmptyItem());
            }
        }

        public void AddItem(Item item, int degree)
        {
            for (int i = 0; i < equippedItems.Count; i++)
            {
                if (IsGridFull())
                {
                    return;
                }
                
                if (equippedItems[i].IsEmpty)
                {
                    equippedItems[i] = new EquippedItem
                    {
                        item = item,
                        degree = degree,
                    };
                    return;
                }
            }
        }

        private bool IsGridFull()
        {
            return equippedItems.Where(item => item.IsEmpty).Any() == false;
        }

        public void AddItem(EquippedItem item)
        {
            AddItem(item.item, item.degree);
        }
        
        public Dictionary<int, EquippedItem> GetCurrentSlotState()
        {
            Dictionary<int, EquippedItem> returnValue = new Dictionary<int, EquippedItem>();

            for (int i = 0; i < equippedItems.Count; i++)
            {
                if (equippedItems[i].IsEmpty)
                {
                    continue;
                }
                returnValue[i] = equippedItems[i];
            }
            return returnValue;
        }

        public EquippedItem GetItemAt(int itemIndex)
        {
            return equippedItems[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            EquippedItem item1 = equippedItems[itemIndex1];
            equippedItems[itemIndex1] = equippedItems[itemIndex2];
            equippedItems[itemIndex2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnSlotUpdated?.Invoke(GetCurrentSlotState());
        }
    }

    [Serializable]
    public struct EquippedItem
    {
        public int degree;
        public Item item;

        public bool IsEmpty => item == null;

        public EquippedItem ChangeDegree(int newDegree)
        {
            return new EquippedItem
            {
                item= this.item,
                degree = newDegree,
            };
        }
        
        public static EquippedItem GetEmptyItem() => new EquippedItem()
        {
            item = null,
            degree = 0,
        };
    }
}