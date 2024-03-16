using UnityEngine;

namespace SlotLogic.UI
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private SlotItem slotItem;
        [SerializeField] private RectTransform rectTransform;

        private void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            slotItem = GetComponentInChildren<SlotItem>();
        }

        public void SetData(Sprite sprite, int degree)
        {
            slotItem.SetData(sprite, degree);
        }

        private void Update()
        {
            rectTransform.anchoredPosition = Input.mousePosition - new Vector3(canvas.pixelRect.width/2, canvas.pixelRect.height/2);
        }
        
        public void Toggle(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}