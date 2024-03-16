using UnityEngine;

namespace SlotLogic.Data
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public int ID => GetInstanceID();
        
        [field: SerializeField] public bool IsMerge { get; set; }
        
        [field: SerializeField] public string Name { get; set; }
        
        [field: SerializeField] public Sprite ItemSprite { get; set; }
    }
}