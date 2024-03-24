using System.Xml;
using TrackLogic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotLogic.UI
{
    public class TrackItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private SplineFollow splineFollow;
        
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

            splineFollow.enabled = false;
        }

        public void SetData(Sprite spriteItem, UniqueId uniqueId)
        {
            image.gameObject.SetActive(true);
            IsEmpty = false;
            image.sprite = spriteItem;
            UniqueId = uniqueId;
            
            splineFollow.enabled = true;
        }
    }
}