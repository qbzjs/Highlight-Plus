using UnityEngine;
using System.Collections.Generic;
using Doublsb.Dialog;
using System.Collections;
using MikelW.Movement;

namespace MikelW.World.Interactions.Dialogue
{
    public class DialogueInteraction : MonoBehaviour, IInteractable
    {
        public string Message;
        public bool MultipleMessages;
        public string[] Messages;

        public DialogManager DialogManager;

        private int messageCount;

        public bool canInteract { get; set; }

        void Start()
        {
            gameObject.layer = 7;
            canInteract = true;
        }

        public void Interaction()
        {
            InteractionSystem.canInteract = false;
            CharController.canMove = false;
            
            var dialogTexts = new List<DialogData>();

            if (MultipleMessages)
            {
                for (int i = 0; i < Messages.Length; i++)
                {
                    dialogTexts.Add(new DialogData(Messages[i]));
                }
            }
            else
            {
                dialogTexts.Add(new DialogData(Message));
            }
            DialogManager.Show(dialogTexts);
            StartCoroutine(CycleDialogue());
            //Open Dialogue and adjust camera here
        }

        public void ResetInteraction()
        {

        }

        private IEnumerator CycleDialogue()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            DialogManager.Click_Window();
            messageCount++;

            if (MultipleMessages && messageCount >= Messages.Length)
            {
                DialogManager.Hide();
                InteractionSystem.canInteract = true;
                CharController.canMove = true;
                messageCount = 0;
            }
            else if(!MultipleMessages)
            {

                DialogManager.Hide();
                InteractionSystem.canInteract = true;
                CharController.canMove = true;
                messageCount = 0;
            }
            else
            {
                StartCoroutine(CycleDialogue());
            }

            yield return null;
        }
    }
}