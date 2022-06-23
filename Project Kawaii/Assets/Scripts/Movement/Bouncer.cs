using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float launchImpulse = 5;
    public bool overrideVerticalVelocity = true;
    public bool overrideLateralVelocity = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered collider is using the CharacterMovement component
        if (other.TryGetComponent(out CharacterMovement characterMovement))
        {
            // If necessary, temporarily disable the character's ground constraint so it leave the ground
        if (characterMovement.isGrounded)
                characterMovement.PauseGroundConstraint();

            // Launch character!
            characterMovement.LaunchCharacter(transform.up * launchImpulse, overrideVerticalVelocity, overrideLateralVelocity);
        }

    }
}