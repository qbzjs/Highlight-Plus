using EasyCharacterMovement;
using UnityEngine;

namespace MikelW.Movement
{
    public class ForceField : MonoBehaviour
    {
        public ForceMode forceMode = ForceMode.Force;
        public float forceMagnitude = 15.0f;
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out CharacterMovement characterMovement))
            {
                if (characterMovement.isGrounded)
                    characterMovement.PauseGroundConstraint();
                characterMovement.AddForce(transform.up * forceMagnitude,
                forceMode);
            }
        }
    }
}