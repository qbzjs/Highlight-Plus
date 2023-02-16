using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MikelW.Menus
{
    public class DragUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private Canvas canvas;

        private Image img;
        private RectTransform rectTransform;
        private Vector2 prevPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            img = GetComponent<Image>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            img.raycastTarget = false;
            prevPosition = rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag");
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUP");
            rectTransform.anchoredPosition = prevPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
            img.raycastTarget = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
        }
    }
}