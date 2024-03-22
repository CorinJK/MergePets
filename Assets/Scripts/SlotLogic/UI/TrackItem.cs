using UnityEngine;
using UnityEngine.UI;

namespace SlotLogic.UI
{
    public class TrackItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private int profit;

        public bool IsEmpty = true;
        
        private void Awake()
        {
            ResetData();
        }

        private void ResetData()
        {
            IsEmpty = true;
            image.gameObject.SetActive(false);
        }

        public void SetData(Sprite spriteItem, int profitItem)
        {
            image.gameObject.SetActive(true);
            IsEmpty = false;
            image.sprite = spriteItem;
            profit = profitItem;
        }
    }
}