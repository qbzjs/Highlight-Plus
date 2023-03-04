using UnityEngine;
using MikelW.GUI;

namespace MikelW.World.Interactions
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField]
        private float distance = 2;

        [SerializeField]
        private Collider col;

        [SerializeField]
        private LayerMask layerToHit;

        private bool hitDetected;
        private RaycastHit rayHit;

        private void Awake()
        {
            col = GetComponent<Collider>();
        }

        void FixedUpdate()
        {
            hitDetected = Physics.BoxCast(col.bounds.center - (transform.forward / 2), transform.localScale / 2, transform.forward, out rayHit, transform.rotation, distance, layerToHit);

            if (hitDetected && rayHit.transform.GetComponent<IInteractable>().canInteract)
                HoverUI.SetText = "Interact with: " + rayHit.transform.gameObject.name + " ? ";
            else
                HoverUI.SetText = "";
        }

        //Draw the BoxCast as a gizmo to show where it currently is testing
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            //Check if there has been a hit yet
            if (hitDetected)
            {
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(col.bounds.center - (transform.forward / 2), transform.forward * rayHit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(col.bounds.center + transform.forward * rayHit.distance, transform.localScale * 1.35f);
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(col.bounds.center - (transform.forward / 2), transform.forward * distance);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(col.bounds.center + transform.forward * distance, transform.localScale * 1.35f);
            }
        }

        public void Interact()
        {
            Debug.Log("Called for an interaction");

            if (Physics.BoxCast(col.bounds.center - (transform.forward / 2), transform.localScale / 2, transform.forward, out rayHit, transform.rotation, distance, layerToHit))
            {
                Debug.Log("Ray Hit:" + rayHit.transform.gameObject.name);

                IInteractable hitInteraction = rayHit.transform.GetComponent<IInteractable>();

                if (hitInteraction.canInteract)
                    rayHit.transform.GetComponent<IInteractable>().Interaction();
            }
            else
                Debug.LogWarning("Didn't hit anything with interaction");
        }
    }
}