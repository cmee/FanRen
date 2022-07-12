using System.Collections;
using UnityEngine;

namespace RPGCharacterAnimsFREE
{
    public enum RPGCharacterState
    {
        Idle = 0,
        Move = 1,
        Jump = 2,
        Fall = 3,
        Knockback = 4,
        DiveRoll = 5
    }

    public class RPGCharacterMovementController : SuperStateMachine
    {
        // Components.
        private SuperCharacterController superCharacterController;
        private RPGCharacterController rpgCharacterController;
        private Rigidbody rb;
        private Animator animator;
        private CapsuleCollider capCollider;

        [HideInInspector] public Vector3 lookDirection { get; private set; }

        /// <summary>
        /// Multiplies the amount of knockback force a character recieves when they get hit.
        /// </summary>
        public float knockbackMultiplier = 1f;

        /// <summary>
        /// Multiplies the speed of animation velocity.
        /// </summary>
        public float movementAnimationMultiplier = 1f;

        /// <summary>
        /// Vector3 movement velocity.
        /// </summary>
        [HideInInspector] public Vector3 currentVelocity;

        [Header("Movement")]
        /// <summary>
        /// Movement speed while walking and strafing.
        /// </summary>
        public float walkSpeed = 0.5f;

        /// <summary>
        /// Walking acceleration.
        /// </summary>
        public float walkAccel = 15f;

        /// <summary>
        /// Movement speed while running. (the default movement)
        /// </summary>
        public float runSpeed = 1f;

        /// <summary>
        /// Running acceleration.
        /// </summary>
        public float runAccel = 30f;

        /// <summary>
        /// Movement speed while injured.
        /// </summary>
        public float injuredSpeed = 0.5f;

        /// <summary>
        /// Acceleration while injured.
        /// </summary>
        public float injuredAccel = 20f;

        /// <summary>
        /// Ground friction, slows the character to a stop.
        /// </summary>
        public float groundFriction = 120f;

        /// <summary>
        /// Speed of rotation when turning the character to face movement direction or target.
        /// </summary>
        public float rotationSpeed = 100f;

        /// <summary>
        /// Internal flag for when the character can jump.
        /// </summary>
        [HideInInspector] public bool canJump;

        /// <summary>
        /// Internal flag for if the player is holding the jump input. If this is released while
        /// the character is still ascending, the vertical speed is damped.
        /// </summary>
        [HideInInspector] public bool holdingJump;

        [Header("Jumping")]
        /// <summary>
        /// Jumping speed while ascending.
        /// </summary>
        public float jumpSpeed = 12f;

        /// <summary>
        /// Gravity while ascending.
        /// </summary>
        public float jumpGravity = 24f;

        /// <summary>
        /// Horizontal speed while in the air.
        /// </summary>
        public float inAirSpeed = 8f;

        /// <summary>
        /// Horizontal acceleration while in the air.
        /// </summary>
        public float inAirAccel = 16f;

        /// <summary>
        /// Gravity while descending. Default is higher than ascending gravity (like a Mario jump).
        /// </summary>
        public float fallGravity = 32f;

		#region Initalization

		private void Awake()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
            rpgCharacterController.SetHandler("AcquiringGround", new Actions.SimpleActionHandler(() => { }, () => { }));
            rpgCharacterController.SetHandler("MaintainingGround", new Actions.SimpleActionHandler(() => { }, () => { }));
            rpgCharacterController.SetHandler("DiveRoll", new Actions.DiveRoll(this));
            rpgCharacterController.SetHandler("Fall", new Actions.Fall(this));
            rpgCharacterController.SetHandler("GetHit", new Actions.GetHit(this));
            rpgCharacterController.SetHandler("Idle", new Actions.Idle(this));
            rpgCharacterController.SetHandler("Jump", new Actions.Jump(this));
            rpgCharacterController.SetHandler("Knockback", new Actions.Knockback(this));
            rpgCharacterController.SetHandler("Move", new Actions.Move(this));
		}

        private void Start()
        {
            // Get other RPG Character components.
            superCharacterController = GetComponent<SuperCharacterController>();

            // Check if Animator exists, otherwise pause script.
            animator = GetComponentInChildren<Animator>();
			if (animator == null) {
				Debug.LogError("ERROR: THERE IS NO ANIMATOR COMPONENT ON CHILD OF CHARACTER.");
				Debug.Break();
			}
			// Setup Collider and Rigidbody for collisions.
			capCollider = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();

            // Set restraints on startup if using Rigidbody.
            if (rb != null) { rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; }
            rpgCharacterController.OnLockMovement += LockMovement;
            rpgCharacterController.OnUnlockMovement += UnlockMovement;
            RPGCharacterAnimatorEvents animatorEvents = rpgCharacterController.GetAnimatorTarget().GetComponent<RPGCharacterAnimatorEvents>();
            animatorEvents.OnMove.AddListener(AnimatorMove);
        }

		#endregion

		#region Updates

		/*
		Update is normally run once on every frame update. We won't be using it in this case, since the SuperCharacterController
        component sends a callback Update called SuperUpdate. SuperUpdate is recieved by the SuperStateMachine, and then fires
        further callbacks depending on the state.

        If SuperCharacterController is disabled then we still want the SuperStateMachine to run, so we call SuperUpdate manually.
        */
		void Update()
        {
            if (!superCharacterController.enabled) { gameObject.SendMessage("SuperUpdate", SendMessageOptions.DontRequireReceiver); }
        }

        // Put any code in here you want to run BEFORE the state's update function. This is run regardless of what state you're in.
        protected override void EarlyGlobalSuperUpdate()
        {
            bool acquiringGround = superCharacterController.currentGround.IsGrounded(false, 0.01f);
            bool maintainingGround = superCharacterController.currentGround.IsGrounded(true, 0.5f);

            if (acquiringGround) { rpgCharacterController.StartAction("AcquiringGround"); } 
			else { rpgCharacterController.EndAction("AcquiringGround"); }

            if (maintainingGround) { rpgCharacterController.StartAction("MaintainingGround"); } 
			else { rpgCharacterController.EndAction("MaintainingGround"); }
        }

        // Put any code in here you want to run AFTER the state's update function.  This is run regardless of what state you're in.
        protected override void LateGlobalSuperUpdate()
        {
            // If the movement controller itself is disabled, this shouldn't run.
            if (!enabled) { return; }

            // Move the player by our velocity every frame.
            transform.position += currentVelocity * superCharacterController.deltaTime;

            // If alive and is moving, set animator.
            if (!rpgCharacterController.isDead && rpgCharacterController.canMove) {
                if (currentVelocity.magnitude > 0f) {
                    animator.SetFloat("Velocity X", 0);
                    animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z * movementAnimationMultiplier);
					animator.SetBool("Moving", true);
				} else {
                    animator.SetFloat("Velocity X", 0f);
                    animator.SetFloat("Velocity Z", 0f);
                    animator.SetBool("Moving", false);
                }
            }
			// Strafing.
			if (rpgCharacterController.isStrafing) { RotateTowardsTarget(rpgCharacterController.aimInput); } 
			else if (rpgCharacterController.canMove) { RotateTowardsMovementDir(); }

            if (currentState == null && rpgCharacterController.CanStartAction("Idle")) { rpgCharacterController.StartAction("Idle"); }

			// Update animator with local movement values.
			animator.SetFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x * movementAnimationMultiplier);
			animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z * movementAnimationMultiplier);
		}

        #endregion

        #region States
        // Below are the state functions. Each one is called based on the name of the state, so when currentState = Idle,
        // we call Idle_EnterState. If currentState = Jump, we call Jump_SuperUpdate().

        private void Idle_EnterState()
        {
            superCharacterController.EnableSlopeLimit();
            superCharacterController.EnableClamping();
            canJump = true;
        }

        // Run every frame character is in the idle state.
        private void Idle_SuperUpdate()
        {
            // Apply friction to slow to a halt.
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction * superCharacterController.deltaTime);

            if (rpgCharacterController.CanStartAction("Move")) { rpgCharacterController.StartAction("Move"); }
        }

        // Run every frame character is moving.
        private void Move_SuperUpdate()
        {
            if (rpgCharacterController.CanStartAction("Fall")) {
                rpgCharacterController.StartAction("Fall");
                return;
            }
            // Set speed determined by movement type.
            if (rpgCharacterController.canMove) {
                float moveSpeed = runSpeed;
                float moveAccel = runAccel;

                if (rpgCharacterController.isInjured) {
                    moveSpeed = injuredSpeed;
                    moveAccel = injuredAccel;
                } else if (rpgCharacterController.isStrafing) {
                    moveSpeed = walkSpeed;
                    moveAccel = walkAccel;
                }
                currentVelocity = Vector3.MoveTowards(currentVelocity, rpgCharacterController.cameraRelativeInput * moveSpeed, moveAccel * superCharacterController.deltaTime);
            }

            if (rpgCharacterController.CanStartAction("Idle")) {  rpgCharacterController.StartAction("Idle"); }
        }

        private void Jump_EnterState()
        {
            superCharacterController.DisableClamping();
            superCharacterController.DisableSlopeLimit();

            currentVelocity = new Vector3(currentVelocity.x, jumpSpeed, currentVelocity.z);

            animator.SetInteger("Jumping", 1);
            rpgCharacterController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
            canJump = false;
        }

        private void Jump_SuperUpdate()
        {
            holdingJump = rpgCharacterController.jumpInput.y != 0f;

            // Cap jump speed if we stop holding the jump button.
            if (!holdingJump && currentVelocity.y > (jumpSpeed / 4f)) {
                currentVelocity = Vector3.MoveTowards(currentVelocity, new Vector3(currentVelocity.x, (jumpSpeed / 4f), currentVelocity.z), fallGravity * superCharacterController.deltaTime);
            }
            Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
            Vector3 verticalMoveDirection = currentVelocity - planarMoveDirection;

            // Falling.
            if (currentVelocity.y < 0) {
                currentVelocity = planarMoveDirection;
                currentState = RPGCharacterState.Fall;
                return;
            }
            planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, rpgCharacterController.cameraRelativeInput * inAirSpeed, inAirAccel * superCharacterController.deltaTime);
            verticalMoveDirection -= superCharacterController.up * jumpGravity * superCharacterController.deltaTime;
            currentVelocity = planarMoveDirection + verticalMoveDirection;
        }

        private void Fall_EnterState()
        {
            superCharacterController.DisableClamping();
            superCharacterController.DisableSlopeLimit();
            canJump = false;
            animator.SetInteger("Jumping", 2);
            rpgCharacterController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
        }

        private void Fall_SuperUpdate()
        {
            if (rpgCharacterController.CanStartAction("Idle")) {
                currentVelocity = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
                rpgCharacterController.StartAction("Idle");
                return;
            }
            currentVelocity -= superCharacterController.up * fallGravity * superCharacterController.deltaTime;
        }

		private void Fall_ExitState()
		{
			animator.SetInteger("Jumping", 0);
			rpgCharacterController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);

			// Character landed.
			if (rpgCharacterController.maintainingGround) { Land(); }
		}

		private void DiveRoll_EnterState()
        {
            rpgCharacterController.OnUnlockMovement += IdleOnceAfterMoveUnlock;
        }

		private void DiveRoll_SuperUpdate()
		{
			if (rpgCharacterController.CanStartAction("Idle")) {
				currentVelocity = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
				rpgCharacterController.StartAction("Idle");
				return;
			}
			currentVelocity -= superCharacterController.up * (fallGravity / 2) * superCharacterController.deltaTime;
		}

		private void Knockback_EnterState()
        {
            rpgCharacterController.OnUnlockMovement += IdleOnceAfterMoveUnlock;
        }

        private void Knockdown_EnterState()
        {
            rpgCharacterController.OnUnlockMovement += IdleOnceAfterMoveUnlock;
        }

		#endregion

		/// <summary>
		/// Set velocity to 0 after character makes contact with the ground after jump or fall.
		/// </summary>
		private void Land()
		{
			currentVelocity = Vector3.zero;
		}

		/// <summary>
		/// Rotates the character to be head up compared to gravity.
		/// </summary>
		/// <param name="up">Up direction. (i.e. Vector3.up)</param>
		public void RotateGravity(Vector3 up)
		{
			lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
		}

		/// <summary>
		/// Rotate towards the direction the character is moving.
		/// </summary>
		private void RotateTowardsMovementDir()
        {
            Vector3 movementVector = new Vector3(currentVelocity.x, 0, currentVelocity.z);
            if (movementVector.magnitude > 0.01f) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), Time.deltaTime * rotationSpeed);
            }
        }

		/// <summary>
		/// Rotate towards a point in space.  Used when Targeting/Strafing.
		/// </summary>
		private void RotateTowardsTarget(Vector3 targetPosition)
        {
			Vector3 lookTarget = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);
			if (lookTarget != Vector3.zero) {
				Quaternion targetRotation = Quaternion.LookRotation(lookTarget);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
			}
        }

		/// <summary>
		/// Exert a knockback force on the character. Used by the GetHit, Knockdown, and Knockback
		/// actions.
		/// </summary>
		/// <param name="knockDirection">Vector3 direction knock the character.</param>
		/// <param name="knockBackAmount">Amount to knock back.</param>
		/// <param name="variableAmount">Random variance in knockback.</param>
		public void KnockbackForce(Vector3 knockDirection, float knockBackAmount, float variableAmount)
        {
            StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
        }

        private IEnumerator _KnockbackForce(Vector3 knockDirection, float knockBackAmount, float variableAmount)
        {
            if (rb == null) { yield break; }

            float startTime = Time.time;
            float elapsed = 0f;

            rb.isKinematic = false;

            while (elapsed < .1f) {
                rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * knockbackMultiplier * 10), ForceMode.Impulse);
                elapsed = Time.time - startTime;
                yield return null;
            }

            rb.isKinematic = true;
        }

        /// <summary>
        /// Event listener for when RPGCharacterController.OnLockMovement is called.
        /// </summary>
        public void LockMovement()
        {
            currentVelocity = new Vector3(0, 0, 0);
            animator.SetBool("Moving", false);
            animator.applyRootMotion = true;
        }

        /// <summary>
        /// Event listener for when RPGCharacterController.OnUnlockMovement is called.
        /// </summary>
        public void UnlockMovement()
        {
            animator.applyRootMotion = false;
        }

        /// <summary>
        /// Event listener for when RPGCharacterAnimatorEvents.OnMove is called.
        /// </summary>
        /// <param name="deltaPosition">Change in position.</param>
        /// <param name="rootRotation">Change in rotation.</param>
        public void AnimatorMove(Vector3 deltaPosition, Quaternion rootRotation)
        {
            transform.position += deltaPosition;
            transform.rotation = rootRotation;
        }

        /// <summary>
        /// Event listener to return to the Idle state once movement is unlocked, which executes
        /// once. Use with the RPGCharacterController.OnUnlockMovement event.
        ///
        /// e.g.: rpgCharacterController.OnUnlockMovement += IdleOnceAfterMoveUnlock;
        /// </summary>
        public void IdleOnceAfterMoveUnlock()
        {
            rpgCharacterController.StartAction("Idle");
            rpgCharacterController.OnUnlockMovement -= IdleOnceAfterMoveUnlock;
        }
    }
}