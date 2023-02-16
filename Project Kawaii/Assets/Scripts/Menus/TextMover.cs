using UnityEngine;

namespace MikelW.Menus
{
    public class TextMover : MonoBehaviour
    {
        public int offsetX = 0, offsetY = 10;
        private RectTransform textRect;
        private Vector3 pos;

        void Start()
        {
            textRect = gameObject.GetComponent<RectTransform>();
            pos = textRect.localPosition;
        }

        public void Down()
        {
            textRect.localPosition = new Vector3(pos.x + offsetX, pos.y - offsetY, pos.z);
        }

        public void Up()
        {
            textRect.localPosition = pos;
        }
    }
}