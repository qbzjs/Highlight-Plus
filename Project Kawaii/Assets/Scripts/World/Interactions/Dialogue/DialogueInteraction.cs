using UnityEngine;

namespace MikelW.World.Interactions.Dialogue
{
    public class DialogueInteraction : MonoBehaviour, IInteractable
    {
        public string Message;
        public bool MultipleMessages;
        public string[] Messages;

        public bool canInteract { get; set; }

        void Start()
        {
            gameObject.layer = 7;
            canInteract = true;
        }

        public void Interaction()
        {
            //Open Dialogue and adjust camera here
        }

        public void ResetInteraction()
        {

        }
    }
}