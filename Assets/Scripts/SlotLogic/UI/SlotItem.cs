using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SlotLogic.UI
{
    public class SlotItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text degreeText;
        
        public event Action<SlotItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag;
        public event Action<PointerEventData> OnItemEndDrag;

        private bool empty = true;

        private void Awake()
        {
            ResetData();
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void SetData(Sprite spriteItem, int degree)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = spriteItem;
            degreeText.text = degree + "";
            empty = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
            {
                return;
            }
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}