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
        [SerializeField] private List<Item> mergeProgress;
        
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

        public void AddItem(Item item)
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
                    };
                    InformAboutChange();
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
            AddItem(item.item);
        }

        public void RemoveItem(int itemIndex)
        {
            if (equippedItems.Count > itemIndex)
            {
                if (equippedItems[itemIndex].IsEmpty)
                {
                    return;
                }
                
                equippedItems[itemIndex] = EquippedItem.GetEmptyItem();
                InformAboutChange();
            }
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

        public void ProgressDegreeItems(int itemIndex1, int itemIndex2)
        {
            EquippedItem item1 = equippedItems[itemIndex1];
            EquippedItem item2 = equippedItems[itemIndex2];

            RemoveItem(itemIndex1);
            RemoveItem(itemIndex2);

            int indexMergeItem = item2.item.ID;
            
            equippedItems[itemIndex2] = new EquippedItem
            {
                item = mergeProgress[indexMergeItem++],
            };

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
        public Item item;

        public bool IsEmpty => item == null;

        public static EquippedItem GetEmptyItem() => new EquippedItem()
        {
            item = null,
        };
    }
}