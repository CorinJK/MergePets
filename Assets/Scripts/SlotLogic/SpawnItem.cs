using SlotLogic.Data;
using UnityEngine;

namespace SlotLogic
{
    public class SpawnItem : MonoBehaviour
    {
        [field: SerializeField] public Item item { get; private set; }
        [SerializeField] private Slot slotData;

        public void AddItem()
        {
            slotData.AddItem(item);
        }
    }
}