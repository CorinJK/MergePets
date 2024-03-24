using System.Collections.Generic;
using SlotLogic.Data;
using SlotLogic.UI;
using UnityEngine;

namespace SlotLogic
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private SlotPage slotPage;
        [SerializeField] private TrackPage trackPage;
        [SerializeField] private CoinCounter coinCounter;
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
                slotPage.UpdateData(item.Key, item.Value.sprite, item.Value.item.ID);
            }
        }

        private void PrepareSlots()
        {
            slotPage.InitializeSlot(slotData.SizeSlot);
            trackPage.InitializeTrackItem(slotData.SizeTrack);
            
            slotPage.OnDropItems += HandleDropItems;
            slotPage.OnStartDrag += HandleDrag;
            slotPage.OnStartRun += HandleStartRun;
            slotPage.OnClick += HandleClick;
            trackPage.OnCountCoins += AccrualСoins;
        }

        private void AccrualСoins(TrackItem trackItem)
        {
            int amountCoin = slotData.GetAmountCoin(trackItem.UniqueId);
            coinCounter.IncreaseCoin(amountCoin);
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
                slotPage.UpdateData(item.Key, item.Value.sprite, item.Value.item.ID);
            }
        }

        private void HandleDrag(int itemIndex)
        {
            EquippedItem equippedItem = slotData.GetItemAt(itemIndex);
            if (equippedItem.IsEmpty || !equippedItem.IsDrag)
            {
                return;
            }

            slotPage.CreateDragItem(equippedItem.sprite, equippedItem.item.ID);
        }

        private void HandleStartRun(int itemIndex)
        {
            EquippedItem equippedItem = slotData.GetItemAt(itemIndex);
            if (equippedItem.IsEmpty || !equippedItem.IsDrag)
            {
                return;
            }
            
            slotData.CreateRunCat(itemIndex);
            trackPage.AddRunCat(equippedItem.item.ItemSprite, equippedItem.UniqueId);
        }

        private void HandleClick(int itemIndex)
        {
            EquippedItem equippedItem = slotData.GetItemAt(itemIndex);
            
            if (!equippedItem.IsEmpty && !equippedItem.IsDrag)
            {
                slotData.ReturnRunCat(itemIndex);
                trackPage.ReturnRunCat(equippedItem.UniqueId);
            }
        }

        private void HandleDropItems(int itemIndex1, int itemIndex2)
        {
            EquippedItem equippedItem1 = slotData.GetItemAt(itemIndex1);
            EquippedItem equippedItem2 = slotData.GetItemAt(itemIndex2);

            if (equippedItem1.IsDrag && equippedItem2.IsDrag)
            {
                if (!equippedItem1.IsEmpty && !equippedItem2.IsEmpty &&
                    equippedItem1.item.ID == equippedItem2.item.ID && itemIndex1 != itemIndex2)
                {
                    slotData.ProgressDegreeItems(itemIndex1, itemIndex2);
                }
                else
                {
                    slotData.SwapItems(itemIndex1, itemIndex2);
                }
            }
        }

        private void OnDisable()
        {
            slotPage.OnDropItems -= HandleDropItems;
            slotPage.OnStartDrag -= HandleDrag;
            slotPage.OnStartRun -= HandleStartRun;
            slotData.OnSlotUpdated -= UpdateSlot;
            trackPage.OnCountCoins -= AccrualСoins;
        }
    }
}