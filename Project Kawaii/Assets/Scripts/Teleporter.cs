using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter destination;
    public bool isTeleporterEnabled;

    public CharacterMovement _characterMovement;
    private void OnTriggerEnter(Collider other)
    {
        // If no destination or this teleporter is disabled, return
        if (destination == null || !isTeleporterEnabled)
            return;
        if (other.TryGetComponent(out _characterMovement))
        {
            // If entered collider is using a CharacterMovement component,
            // disable interpolation and update character position
            _characterMovement.interpolation = RigidbodyInterpolation.None;
            _characterMovement.SetPosition(destination.transform.position, true);
            // Disable destination teleporter until teleported character left it,
            // otherwise will be teleported back in an infinite loop!
            destination.isTeleporterEnabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // On left, make sure teleporter is re-enabled
        isTeleporterEnabled = true;
        if (other.TryGetComponent(out CharacterMovement characterMovement) && characterMovement == _characterMovement)
        {
            // Character has been teleported from this teleporter,
            // Re-enable interpolation setting and clear cached component
            _characterMovement.interpolation = RigidbodyInterpolation.Interpolate;
            _characterMovement = null;
        }

    }
}
