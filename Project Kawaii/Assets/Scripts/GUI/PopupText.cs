using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MikelW.GUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PopupText : MonoBehaviour
    {
        [Tooltip("Images for text such as a panel or icon")]
        public Image main;

        [Tooltip("Images for text such as a panel or icon")]
        public Image background;

        public static string screenText = "";

        private float timerForMessages;
        private bool cStarted;
        private List<string> messagesToDo;

        private TextMeshProUGUI textMesh;
        private Animator textAnimator;

        void Start()
        {
            messagesToDo = new List<string>(new string[0]);
            textMesh = GetComponent<TextMeshProUGUI>();
            textAnimator = GetComponent<Animator>();
            textMesh.text = "";
            timerForMessages = textAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        void Update()
        {
            HandleText();
        }

        private void HandleText()
        {
            if (screenText != "")
            {
                messagesToDo.Add(screenText);
                screenText = "";
            }
            if (cStarted == false && messagesToDo.Count > 0)
            {
                StartCoroutine(SetText());
                if (main != null)
                    main.enabled = true;
                if (background != null)
                    background.enabled = true;
            }
            else if (messagesToDo.Count <= 0)
            {
                if (main != null)
                    main.enabled = false;
                if (background != null)
                    background.enabled = false;
            }

            if (textMesh.text != "")
            {
                if (main != null)
                    main.enabled = true;
                if (background != null)
                    background.enabled = true;
            }
            else
            {
                if (main != null)
                    main.enabled = false;
                if (background != null)
                    background.enabled = false;
            }

        }

        IEnumerator SetText()
        {
            //Prevent Multiple Coroutines from running
            cStarted = true;
            for (int i = 0; i < messagesToDo.Count; i++)
            {
                //Sets text back to empty to avoid animation issues
                textMesh.text = "";
                //Trigger default text animation
                textAnimator.SetTrigger("Default");
                //Setting text to oldest (first) message
                textMesh.text = messagesToDo[0];
                //Wait time between messages
                yield return new WaitForSeconds(timerForMessages);
                //Removes shown message
                messagesToDo.Remove(messagesToDo[0]);
            }
            //Setting the text back to blank
            textMesh.text = "";
            //Allows the next to start
            cStarted = false;
        }
    }
}