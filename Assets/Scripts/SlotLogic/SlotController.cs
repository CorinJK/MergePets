using System.Collections.Generic;
using SlotLogic.Data;
using SlotLogic.UI;
using UnityEngine;

namespace SlotLogic
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private SlotPage slotPage;
        [SerializeField] private Slot slotData;

        public List<EquippedItem> equippedItems = new List<EquippedItem>();

        private void Awake()
        {
            PrepareSlots();
            PrepareSlotsData();
        }

        private void Update()
        {
            foreach (var item in slotData.GetCurrentSlotState())
            {
                slotPage.UpdateData(item.Key, item.Value.item.ItemSprite, item.Value.degree);
            }
        }

        private void PrepareSlots()
        {
            slotPage.InitializeSlot(slotData.Size);
            slotPage.OnSwapItems += HandleSwapItems;
            slotPage.OnStartDrag += HandleDrag;
        }

        private void PrepareSlotsData()
        {
            slotData.Initialize();
            slotData.OnSlotUpdated += UpdateSlot;

            foreach (EquippedItem item in equippedItems)
            {
                if (item.IsEmpty)
                {
                    continue;
                }
                slotData.AddItem(item);
            }
        }

        private void UpdateSlot(Dictionary<int, EquippedItem> slotState)
        {
            slotPage.ResetAllItems();

            foreach (var item in slotState)
            {
                slotPage.UpdateData(item.Key, item.Value.item.ItemSprite, item.Value.degree);
            }
        }

        private void HandleDrag(int itemIndex)
        {
            EquippedItem equippedItem = slotData.GetItemAt(itemIndex);
            if (equippedItem.IsEmpty)
            {
                return;
            }

            slotPage.CreateDragItem(equippedItem.item.ItemSprite, equippedItem.degree);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            slotData.SwapItems(itemIndex_1, itemIndex_2);
        }
    }
}