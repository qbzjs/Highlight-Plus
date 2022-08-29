using EasyCharacterMovement;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    #region Variables
    public InputActionAsset inputActions;

    [Space(15f)]
    [Tooltip("The Player following camera.")]
    public Camera playerCamera;

    [Tooltip("Change in rotation per second (Deg / s).")]
    public float rotationRate = 540.0f;

    [Space(15f)]
    [Tooltip("The character's maximum speed.")]
    public float maxSpeed = 5.0f;

    [Tooltip("Max Acceleration (rate of change of velocity).")]
    public float maxAcceleration = 20.0f;

    [Tooltip("Setting that affects movement control. Higher values allow faster changes in direction.")]
    public float groundFriction = 8.0f;

    [Space(15f)]
    [Tooltip("Initial velocity (instantaneous vertical velocity) when jumping.")]
    public float jumpImpulse = 6.5f;

    [Tooltip("Friction to apply when falling.")]
    public float airFriction = 0.1f;

    [Range(0.0f, 1.0f)]
    [Tooltip("When falling, amount of horizontal movement control available to the character.\n" +
             "0 = no control, 1 = full control at max acceleration.")]
    public float airControl = 0.3f;

    [Tooltip("The character's gravity.")]
    public Vector3 gravity = Vector3.down * 9.81f;

    [Space(15f)]
    [Tooltip("Character's height when standing.")]
    public float standingHeight = 2.0f;

    [Tooltip("Character's height when crouching.")]
    public float crouchingHeight = 1.25f;

    [Tooltip("The max speed modifier while crouching.")]
    [Range(0.0f, 1.0f)]
    public float crouchSpeedModifier = 0.5f;

    [Tooltip("The max speed modifier while crouching.")]
    [Range(0.0f, 1.0f)]
    public float sprintSpeedModifier = 1.5f;

    private Coroutine _lateFixedUpdateCoroutine;

    public CharacterMovement characterMovement { get; private set; }

    public Vector3 movementDirection { get; set; }

    public bool jump { get; set; }

    public bool crouch { get; set; }

    public bool sprint { get; set; }

    public bool isCrouching { get; protected set; }
    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        // Cache CharacterMovement component
        characterMovement = GetComponent<CharacterMovement>();

        // Enable default physic interactions
        characterMovement.enablePhysicsInteraction = true;
    }

    private void OnEnable()
    {
        // Start LateFixedUpdate coroutine
        if (_lateFixedUpdateCoroutine != null)
            StopCoroutine(_lateFixedUpdateCoroutine);

        _lateFixedUpdateCoroutine = StartCoroutine(LateFixedUpdate());

        // Subscribe to CharacterMovement events
        characterMovement.FoundGround += OnFoundGround;
        characterMovement.Collided += OnCollided;

        //Collision Filter
        characterMovement.colliderFilterCallback += ColliderFilterCallback;

        //Callback
        characterMovement.collisionBehaviorCallback += CollisionBehaviorCallback;
    }

    private void OnDisable()
    {
        // Ends LateFixedUpdate coroutine
        if (_lateFixedUpdateCoroutine != null)
            StopCoroutine(_lateFixedUpdateCoroutine);

        // Un-Subscribe from CharacterMovement events
        characterMovement.FoundGround -= OnFoundGround;
        characterMovement.Collided -= OnCollided;

        //Collision Filter
        characterMovement.colliderFilterCallback -= ColliderFilterCallback;

        //Callback
        characterMovement.collisionBehaviorCallback -= CollisionBehaviorCallback;
    }
    #endregion Unity Methods

    #region OnEvents
    private void OnCollided(ref CollisionResult inHit)
    {
        Debug.Log($"{name} collided with: {inHit.collider.name}");
    }

    private void OnFoundGround(ref FindGroundResult foundGround)
    {
        Debug.Log("Found ground...");

        // Determine if the character has landed
        if (!characterMovement.wasOnGround && foundGround.isWalkableGround)
        {
            Debug.Log("Landed!");
        }
    }

    /// <summary>
    /// Post-Physics update, used to sync our character with physics.
    /// </summary>
    private void OnLateFixedUpdate()
    {
        UpdateRotation();
        Move();
    }

    private IEnumerator LateFixedUpdate()
    {
        WaitForFixedUpdate waitTime = new WaitForFixedUpdate();

        while (true)
        {
            yield return waitTime;

            OnLateFixedUpdate();
        }
    }
    #endregion OnEvents

    #region Input Methods
    private void OnMovement(InputValue value)
    {
        /* 
        //Ignore other characters collision
        public void IgnoreCollision(Collider otherCollider, bool ignore = true);
        public void IgnoreCollision(Rigidbody otherRigidbody, bool ignore = true)
        //The former specifically ignores collisions against the given collider, while the latter will
        //ignore all the colliders attached to the given Rigidbody.
        */

        Debug.Log("On movement");
        //Get Input
        Vector2 inputPressed = value.Get<Vector2>();

        //Read Input values
        float horizontal = inputPressed.x;
        float vertical = inputPressed.y;

        //Create a Movement direction vector(in world space)
        movementDirection = Vector3.zero;
        movementDirection += Vector3.forward * vertical;
        movementDirection += Vector3.right * horizontal;

        //Make Sure it won't move faster diagonally
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);

        //Make movementDirection relative to camera view direction
        movementDirection = movementDirection.relativeTo(playerCamera.transform);
    }

    private void OnJump(InputValue value)
    {
        Debug.Log("On jump");
        jump = value.isPressed;
    }

    private void OnCrouch(InputValue value)
    {
        Debug.Log("On crouch");
        crouch = value.isPressed;
    }

    private void OnSprint(InputValue value)
    {
        Debug.Log("On sprint");
        sprint = value.isPressed;
    }
    #endregion Input Methods

    #region Movement Methods
    private void UpdateRotation()
    {
        characterMovement.RotateTowards(movementDirection, rotationRate * Time.deltaTime);
    }

    private void GroundedMovement(Vector3 desiredVelocity)
    {
        characterMovement.velocity = Vector3.Lerp(characterMovement.velocity, desiredVelocity,
            1f - Mathf.Exp(-groundFriction * Time.deltaTime));
    }

    private void NotGroundedMovement(Vector3 desiredVelocity)
    {
        Vector3 velocity = characterMovement.velocity;

        // If moving into non-walkable ground, limit its contribution.
        // Allow movement parallel, but not into it because that may push us up.
        if (characterMovement.isOnGround && Vector3.Dot(desiredVelocity, characterMovement.groundNormal) < 0.0f)
        {
            Vector3 groundNormal = characterMovement.groundNormal;
            Vector3 groundNormal2D = groundNormal.onlyXZ().normalized;

            desiredVelocity = desiredVelocity.projectedOnPlane(groundNormal2D);
        }

        if (desiredVelocity != Vector3.zero)
        {
            // Accelerate horizontal velocity towards desired velocity
            Vector3 horizontalVelocity = Vector3.MoveTowards(velocity.onlyXZ(), desiredVelocity,
                maxAcceleration * airControl * Time.deltaTime);

            // Update velocity preserving gravity effects (vertical velocity)
            velocity = horizontalVelocity + velocity.onlyY();
        }

        // Apply gravity
        velocity += gravity * Time.deltaTime;

        // Apply Air friction (Drag)
        velocity -= velocity * airFriction * Time.deltaTime;

        // Update character's velocity
        characterMovement.velocity = velocity;
    }

    private void Crouching()
    {
        if (crouch)
        {
            if (isCrouching)
                return;

            characterMovement.SetHeight(crouchingHeight);
            isCrouching = true;
        }
        else
        {
            if (!isCrouching)
                return;

            if (!characterMovement.CheckHeight(standingHeight))
            {
                characterMovement.SetHeight(standingHeight);
                isCrouching = false;
            }
        }
    }

    private void Jumping()
    {
        if (jump && characterMovement.isGrounded)
        {
            jump = false;

            // Pause ground constraint so character can jump off ground
            characterMovement.PauseGroundConstraint();

            // perform the jump
            Vector3 jumpVelocity = Vector3.up * jumpImpulse;
            characterMovement.LaunchCharacter(jumpVelocity, true);
        }
    }

    private void Move()
    {
        float targetSpeed = isCrouching ? maxSpeed * crouchSpeedModifier : maxSpeed;
        targetSpeed = sprint ? maxSpeed * sprintSpeedModifier : maxSpeed;
        Vector3 desiredVelocity = movementDirection * targetSpeed;

        // Update character's velocity based on its grounding status
        if (characterMovement.isGrounded)
            GroundedMovement(desiredVelocity);
        else
            NotGroundedMovement(desiredVelocity);

        Jumping();
        Crouching();

        characterMovement.Move();

        //Simple Move EXAMPLE
        /*
        float actualMaxSpeed = isCrouching ? maxSpeed * crouchingSpeedModifier : maxSpeed;
        float actualAcceleration = characterMovement.isGrounded ? acceleration : acceleration * airControl;
        float actualBrakingDeceleration = characterMovement.isGrounded ? deceleration : 0.0f;
        float actualFriction = characterMovement.isGrounded ? groundFriction : airFriction;
        float actualBrakingFriction = useSeparateBrakingFriction ? brakingFriction : GetFriction();

        Vector3 desiredVelocity = movementDirection * actualMaxSpeed;
        characterMovement.SimpleMove(desiredVelocity, actualMaxSpeed, actualAcceleration, actualBrakingDeceleration, actualFriction, actualBrakingFriction, gravity);
        */
    }
    #endregion Movement Methods

    #region Callbacks
    private bool ColliderFilterCallback(Collider collider)
    {
        //Debug.Log("ColliderFilter");

        // determine if it should ignore collisions against the given collider(return true) or allow collisions against it(return false)
        // If collider is a character (e.g. using CharacterMovement component)
        // ignore collisions with it (e.g. filter it)
        if (collider.TryGetComponent(out CharacterMovement _))
            return true;
        return false;

    }

    private CollisionBehavior CollisionBehaviorCallback(Collider collider)
    {
        //Debug.Log("CollisionBehavior");

        // Assumes default behavior
        CollisionBehavior collisionBehavior = CollisionBehavior.Default;

        // If the collider is a dynamic Rigidbody, prevent using it as a moving platform(ride on) and disable step up on it(climbable step)
        Rigidbody rb = collider.attachedRigidbody;
        if (rb && !rb.isKinematic)
            collisionBehavior = CollisionBehavior.CanNotRideOn |
            CollisionBehavior.CanNotStepOn;
        return collisionBehavior;
    }
    #endregion Callbacks
}