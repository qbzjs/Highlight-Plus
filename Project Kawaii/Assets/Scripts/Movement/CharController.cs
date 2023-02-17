using EasyCharacterMovement;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MikelW.Movement
{
    /// <summary>
    /// This Class Contains Input Handling And Movement Related Code
    /// </summary>
    public class CharController : MonoBehaviour
    {
        #region Variables
        [Header("Base Requirements")]
        [Tooltip("Game Manager script")]
        [SerializeField]
        private GameManager gameManager;

        [Tooltip("Input asset for player control")]
        [SerializeField]
        private InputActionAsset inputActions;

        [Tooltip("The Player following camera.")]
        [SerializeField]
        private Camera playerCamera;

        [Tooltip("Root gameobject holding inventory UI")]
        [SerializeField]
        private GameObject inventoryCanvas;

        [Tooltip("Root gameobject holding pause menu UI")]
        [SerializeField]
        private GameObject pauseCanvas;

        [Header("Core Movement Settings")]
        [Tooltip("Change in rotation per second (Deg / s)")]
        public float rotationRate = 540.0f;

        [Tooltip("The character's maximum speed.")]
        public float maxSpeed = 5.0f;

        [Tooltip("Max Acceleration (rate of change of velocity)")]
        public float maxAcceleration = 20.0f;

        [Tooltip("The max speed modifier sprinting")]
        [Range(0.0f, 2.0f)]
        public float sprintSpeedModifier = 1.5f;

        [Tooltip("Setting that affects movement control. Higher values allow faster changes in direction")]
        public float groundFriction = 8.0f;

        [Tooltip("Initial velocity (instantaneous vertical velocity) when jumping")]
        public float jumpImpulse = 6.5f;

        [Tooltip("Initial velocity (instantaneous forward velocity) when dashing")]
        public float dashImpulse = 5f;

        [Tooltip("Max amount of jumps before having to land")]
        public int maxJumpCount = 2;

        [Tooltip("Max amount of jumps before having to land")]
        public int maxDashCount = 1;

        [Tooltip("Friction to apply when falling")]
        public float airFriction = 0.1f;

        [Range(0.0f, 1.0f)]
        [Tooltip("When falling, amount of horizontal movement control available to the character.\n" +
                 "0 = no control, 1 = full control at max acceleration")]
        public float airControl = 0.25f;

        [Range(0.0f, 1.0f)]
        [Tooltip("When falling, amount of horizontal movement control available to the character.\n" +
                 "0 = no control, 1 = full control at max acceleration")]
        public float airTurnControl = 0.25f;

        [Tooltip("The character's gravity.")]
        public Vector3 gravity = Vector3.down * 9.81f;

        [Tooltip("Character's height when standing")]
        public float standingHeight = 2.0f;

        [Header("Movement Events")]
        [Tooltip("Activated once when player jumps, can be used mid air")]
        public UnityEvent onJumping;

        [Tooltip("Activated when the player inputs movement, will be called every button press")]
        public UnityEvent onMoving;

        [Tooltip("Activated once when player activates sprint")]
        public UnityEvent onSprinting;

        [Tooltip("Activated once when player dashes")]
        public UnityEvent onDashing;

        [Tooltip("Activated once when player interacts with something")]
        public UnityEvent onInteract;

        [Tooltip("Activated once when player uses an item")]
        public UnityEvent onAction;

        private int currJumpCount = 0;

        private int currDashCount = 0;

        private Coroutine _lateFixedUpdateCoroutine;

        private CharAnimController playerAnim;

        public CharacterMovement characterMovement { get; private set; }

        public Vector3 movementDirection { get; set; }

        public bool jump { get; set; }

        public bool dash { get; set; }

        public bool sprint { get; set; }

        //public bool isCrouching { get; protected set; }
        #endregion Variables

        #region Unity Methods
        private void Awake()
        {
            // Cache CharacterMovement component
            characterMovement = GetComponent<CharacterMovement>();

            // Enable default physic interactions
            characterMovement.enablePhysicsInteraction = true;

        }

        private void Start()
        {
            playerAnim = gameManager.GetPlayerObject().GetComponent<CharAnimController>();
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
        private void Update()
        {
            if (!pauseCanvas.activeInHierarchy)
                HandleInput();
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
        private void HandleInput()
        {
            //Read Input values
            float horizontal = Input.GetAxisRaw($"Horizontal");
            float vertical = Input.GetAxisRaw($"Vertical");

            // Create a movement direction vector (in world space)
            movementDirection = Vector3.zero;
            movementDirection += Vector3.forward * vertical;
            movementDirection += Vector3.right * horizontal;

            // Make movementDirection vector relative to camera view direction
            movementDirection = movementDirection.relativeTo(playerCamera.transform);

            // Make Sure it won't move faster diagonally
            movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);

            // Set Animation Variable
            if(!sprint)
            playerAnim.movement = movementDirection.magnitude / 2;
            else
                playerAnim.movement = movementDirection.magnitude;

            if (movementDirection.magnitude != 0)
            {
                playerAnim.isMoving = true;
                playerAnim.OnAnimChange();
            }
            else
            {
                playerAnim.isMoving = false;
                playerAnim.OnAnimChange();
            }
        }

        private void OnMovement(InputValue value)
        {
            if (!pauseCanvas.activeInHierarchy && (value.Get<Vector2>().x != 0 || value.Get<Vector2>().y != 0))
            {
                Debug.Log("On movement");
                onMoving.Invoke();
            }
        }

        private void OnInteract()
        {
            Debug.Log("On interact");
            onInteract.Invoke();
        }

        private void OnAction()
        {
            Debug.Log("On action");
            onAction.Invoke();
        }

        private void OnJump()
        {
            if (!pauseCanvas.activeInHierarchy && currJumpCount < maxJumpCount && !jump)
            {
                jump = true;
                playerAnim.jump = true;
                playerAnim.OnAnimChange();
                Debug.Log("On jump");
                onJumping.Invoke();
            }
        }

        private void OnDash()
        {
            if (!pauseCanvas.activeInHierarchy && currDashCount < maxDashCount && !dash)
            {
                dash = true;
                playerAnim.dash = true;
                playerAnim.OnAnimChange();
                Debug.Log("On Dash");
                onDashing.Invoke();
            }
        }

        private void OnSprint()
        {
            if (!pauseCanvas.activeInHierarchy)
            {
                sprint = !sprint;
                Debug.Log("On sprint");
                onSprinting.Invoke();
            }
        }

        private void OnPause()
        {
            pauseCanvas.SetActive(!pauseCanvas.activeInHierarchy);
            switch (Time.timeScale)
            {
                case 0:
                    Time.timeScale = 1;
                    break;
                case 1:
                    Time.timeScale = 0;
                    break;
            }
        }

        private void OnOpenInventory()
        {
            if (!pauseCanvas.activeInHierarchy)
                inventoryCanvas.SetActive(!inventoryCanvas.activeInHierarchy);
        }
        #endregion Input Methods

        #region Movement Methods
        private void UpdateRotation()
        {
            // Rotate towards character's movement direction
            if (characterMovement.isGrounded)
                characterMovement.RotateTowards(movementDirection, rotationRate * Time.deltaTime);
            else
                characterMovement.RotateTowards(movementDirection, rotationRate * airTurnControl * Time.deltaTime);
        }

        private void GroundedMovement(Vector3 desiredVelocity)
        {
            characterMovement.velocity = Vector3.Lerp(characterMovement.velocity,
                desiredVelocity, 1f - Mathf.Exp(-groundFriction * Time.deltaTime));
        }

        private void NotGroundedMovement(Vector3 desiredVelocity)
        {
            // Current character's velocity
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

        private void Dashing()
        {
            if (dash && currDashCount < maxDashCount)
            {
                dash = false;
                currDashCount++;

                if (characterMovement.isGrounded)
                {
                    // Pause ground constraint so character can jump off ground
                    characterMovement.PauseGroundConstraint();
                }

                // perform the dash
                Vector3 dashVelocity = (transform.forward * dashImpulse) + (transform.up * (jumpImpulse * 0.2f));
                characterMovement.LaunchCharacter(dashVelocity, true);
            }
        }

        private void Jumping()
        {
            if (jump && currJumpCount < maxJumpCount)
            {
                jump = false;
                currJumpCount++;

                if (characterMovement.isGrounded)
                {
                    // Pause ground constraint so character can jump off ground
                    characterMovement.PauseGroundConstraint();
                }

                // perform the jump
                Vector3 jumpVelocity = Vector3.up * jumpImpulse;
                characterMovement.LaunchCharacter(jumpVelocity, true);
            }
        }

        private void Move()
        {
            Jumping();

            Dashing();

            float targetSpeed = 0;
            //Change speed for sprinting
            if (sprint)
                targetSpeed = sprint ? maxSpeed * sprintSpeedModifier : maxSpeed;
            else
                targetSpeed = maxSpeed;


            // Create our desired velocity using the previously created movement direction vector
            Vector3 desiredVelocity = movementDirection * targetSpeed;
            
            // Update character's velocity based on its grounding status
            if (characterMovement.isGrounded)
            {
                playerAnim.isGrounded = true;
                playerAnim.OnAnimChange();
                currJumpCount = 0;
                currDashCount = 0;
                GroundedMovement(desiredVelocity);
            }
            else
            {
                NotGroundedMovement(desiredVelocity);
                playerAnim.isGrounded = false;
                playerAnim.OnAnimChange();
            }

            // Perform movement using character's current velocity
            characterMovement.Move();
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
}