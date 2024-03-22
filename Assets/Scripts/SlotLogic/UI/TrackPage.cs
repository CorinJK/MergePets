using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlotLogic.UI
{
    public class TrackPage : MonoBehaviour, IDropHandler
    {
        [SerializeField] private TrackItem trackItemPrefab;
        [SerializeField] private RectTransform track;

        [SerializeField] private TMP_Text maxCountRunText;
        [SerializeField] private TMP_Text currentCountRunText;
        private int currentCountRun;
        
        private List<TrackItem> listOfTrackRun = new List<TrackItem>();
        
        public event Action OnItemStartRun;
        
        public void InitializeTrackItem(int slotCount)
        {
            maxCountRunText.text = slotCount.ToString();
            
            for (int i = 0; i < slotCount; i++)
            {
                TrackItem trackItem = Instantiate(trackItemPrefab, track.transform.position, Quaternion.identity);
                trackItem.transform.SetParent(track);
                listOfTrackRun.Add(trackItem);
            }
        }

        public void AddRunCat(Sprite sprite, int profit)
        {
            for (int i = 0; i < listOfTrackRun.Count; i++)
            {
                if (IsGridFull())
                {
                    return;
                }
                
                if (listOfTrackRun[i].IsEmpty)
                {
                    currentCountRun ++;
                    currentCountRunText.text = currentCountRun.ToString();
                    
                    listOfTrackRun[i].SetData(sprite, profit);
                    
                    return;
                }
            }
        }

        private bool IsGridFull()
        {
            return listOfTrackRun.Where(item => item.IsEmpty).Any() == false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                OnItemStartRun?.Invoke();
            }
        }
    }
}