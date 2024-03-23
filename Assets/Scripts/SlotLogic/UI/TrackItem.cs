using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace SlotLogic.UI
{
    public class TrackItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private int profit;

        public UniqueId UniqueId;
        public bool IsEmpty = true;
        
        private void Awake()
        {
            ResetData();
        }

        public void ResetData()
        {
            IsEmpty = true;
            UniqueId = null;
            image.gameObject.SetActive(false);
        }

        public void SetData(Sprite spriteItem, int profitItem, UniqueId uniqueId)
        {
            image.gameObject.SetActive(true);
            IsEmpty = false;
            image.sprite = spriteItem;
            profit = profitItem;
            UniqueId = uniqueId;
        }
    }
}