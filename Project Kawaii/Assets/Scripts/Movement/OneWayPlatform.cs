using EasyCharacterMovement;
using UnityEngine;

namespace MikelW.Movement
{
    public class OneWayPlatform : MonoBehaviour
    {
        public Collider platform;

        private void OnTriggerEnter(Collider other)
        {
            // If the entered collider is using the CharacterMovement component,
            // make the character ignore the platform collider
            if (other.TryGetComponent(out CharacterMovement characterMovement))
                characterMovement.IgnoreCollision(platform);
        }

        private void OnTriggerExit(Collider other)
        {
            // Re-enable collisions against the platform when character leaves the trigger volume
            if (other.TryGetComponent(out CharacterMovement characterMovement))
                characterMovement.IgnoreCollision(platform, false);

        }
    }
}