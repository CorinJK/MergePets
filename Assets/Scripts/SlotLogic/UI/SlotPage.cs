using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotLogic.UI
{
    public class SlotPage : MonoBehaviour
    {
        [SerializeField] private SlotItem slotPrefab;
        [SerializeField] private MouseFollower mouseFollower;
        
        [SerializeField] private RectTransform grid;
        [SerializeField] private Sprite image, image2;

        public event Action<int> OnStartDrag;
        public event Action<int, int> OnSwapItems;
        
        private List<SlotItem> listOfSlots = new List<SlotItem>();

        private int currentDragItem = -1;
        
        public void InitializeSlot(int slotCount)
        {
            for (int i = 0; i < slotCount; i++)
            {
                SlotItem slotItem = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
                slotItem.transform.SetParent(grid);
                listOfSlots.Add(slotItem);

                slotItem.OnItemClicked += HandleItemSelection;
                slotItem.OnItemBeginDrag += HandleBeginDrag;
                slotItem.OnItemDroppedOn += HandleSwamp;
                slotItem.OnItemEndDrag += HandleEndDrag;
            }
        }

        public void UpdateData(int itemIndex, Sprite itemSprite, int degree)
        {
            if (listOfSlots.Count > itemIndex)
            {
                listOfSlots[itemIndex].SetData(itemSprite, degree);
            }
        }
        
        private void Awake()
        {
            mouseFollower.Toggle(false);
        }

        private void Start()
        {
            // listOfSlots[0].SetData(image, 1);
            // listOfSlots[1].SetData(image2, 2);
        }

        private void HandleSwamp(SlotItem slotItem)
        {
            int index = listOfSlots.IndexOf(slotItem);
            if (index == -1)
            {
                return;
            }
            
            OnSwapItems?.Invoke(currentDragItem, index);
        }

        private void HandleBeginDrag(SlotItem slotItem)
        {
            int index = listOfSlots.IndexOf(slotItem);
            if (index == -1)
            {
                return;
            }
            currentDragItem = index;
            HandleItemSelection(slotItem);
            OnStartDrag?.Invoke(index);
        }

        public void CreateDragItem(Sprite sprite, int degree)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, degree);
        }
        
        private void HandleEndDrag(SlotItem slotItem)
        {
            ResetDragItem();
        }

        private void HandleItemSelection(SlotItem slotItem)
        {
            Debug.Log("Click");
        }

        private void ResetDragItem()
        {
            mouseFollower.Toggle(false);
            currentDragItem = -1;
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfSlots)
            {
                item.ResetData();
            }
        }
    }
}