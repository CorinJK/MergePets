using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;

namespace SlotLogic.Data
{
    [CreateAssetMenu]
    public class Slot : ScriptableObject
    {
        [SerializeField] private List<EquippedItem> equippedItems;
        [SerializeField] private List<Item> mergeProgress;
        
        [field: SerializeField] public int SizeSlot { get; private set; } = 6;
        [field: SerializeField] public int SizeTrack { get; private set; } = 3;

        public event Action<Dictionary<int, EquippedItem>> OnSlotUpdated;
        
        public void Initialize()
        {
            equippedItems = new List<EquippedItem>();
            for (int i = 0; i < SizeSlot; i++)
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
                        sprite = item.ItemSprite,
                        IsDrag = true,
                        UniqueId = new UniqueId(),
                    };
                    InformAboutChange();
                    return;
                }
            }
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

        public void CreateRunCat(int itemIndex)
        {
            EquippedItem item = equippedItems[itemIndex];
            RemoveItem(itemIndex);
            
            if (item.IsDrag)
            {
                equippedItems[itemIndex] = new EquippedItem
                {
                    item = item.item,
                    sprite = item.item.ItemSpriteOff,
                    IsDrag = false,
                    UniqueId = item.UniqueId,
                };
                
                InformAboutChange();
            }
        }

        public void ReturnRunCat(int itemIndex)
        {
            EquippedItem item = equippedItems[itemIndex];
            RemoveItem(itemIndex);
            
            if (!item.IsDrag)
            {
                equippedItems[itemIndex] = new EquippedItem
                {
                    item = item.item,
                    sprite = item.item.ItemSprite,
                    IsDrag = true,
                    UniqueId = item.UniqueId,
                };
                
                InformAboutChange();
            }
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
            Item item = mergeProgress[indexMergeItem++];
            
            equippedItems[itemIndex2] = new EquippedItem
            {
                item = item,
                sprite = item.ItemSprite,
                IsDrag = true,
                UniqueId = new UniqueId(),
            };

            InformAboutChange();
        }

        private bool IsGridFull()
        {
            return equippedItems.Where(item => item.IsEmpty).Any() == false;
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
        public Sprite sprite;
        public bool IsDrag;
        public UniqueId UniqueId;
        
        public bool IsEmpty => item == null;
        
        public static EquippedItem GetEmptyItem() => new EquippedItem()
        {
            item = null,
            sprite = null,
            IsDrag = true,
            UniqueId = null,
        };
    }
}