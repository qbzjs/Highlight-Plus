using UnityEngine;
using TMPro;

namespace MikelW.GUI
{
    public class HoverUI : MonoBehaviour
    {
        public static string SetText;

        [SerializeField]
        private TMP_Text textUI;

        private void Update()
        {
            if (SetText != "")
            {
                textUI.text = SetText;
            }
            else
                textUI.text = "";
        }
    }
}