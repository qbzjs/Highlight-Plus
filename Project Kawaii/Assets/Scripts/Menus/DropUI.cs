using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MikelW.Menus
{
    public class DropUI : MonoBehaviour, IDropHandler
    {
        public UnityEvent onDrop;

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop");
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                onDrop.Invoke();
            }
        }
    }
}