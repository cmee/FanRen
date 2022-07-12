using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCharacterAnimsFREE.Actions;

namespace RPGCharacterAnimsFREE
{
    /// <summary>
    /// RPGCharacterController is the main entry point for triggering animations and holds all the
    /// state related to a character. It is the core component of this package–no other controller
    /// will run without it.
    /// </summary>
    public class RPGCharacterController : MonoBehaviour
    {
        /// <summary>
        /// Event called when actions are locked by an animation.
        /// </summary>
        public event System.Action OnLockActions = delegate { };

        /// <summary>
        /// Event called when actions are unlocked at the end of an animation.
        /// </summary>
        public event System.Action OnUnlockActions = delegate { };

        /// <summary>
        /// Event called when movement is locked by an animation.
        /// </summary>
        public event System.Action OnLockMovement = delegate { };

        /// <summary>
        /// Event called when movement is unlocked at the end of an animation.
        /// </summary>
        public event System.Action OnUnlockMovement = delegate { };

        /// <summary>
        /// Unity Animator component.
        /// </summary>
        [HideInInspector] public Animator animator;

        /// <summary>
        /// Animation speed control. Doesn't affect lock timing.
        /// </summary>
        public float animationSpeed = 1;

		/// <summary>
		/// IKHands component.
		/// </summary>
		[HideInInspector] public IKHands ikHands;

		/// <summary>
		/// Target for Aiming/Strafing.
		/// </summary>
		public Transform target;

		/// <summary>
		/// Returns whether the character can take actions.
		/// </summary>
		public bool canAction { get { return _canAction && !isDead; } }
        private bool _canAction;

        /// <summary>
        /// Returns whether the character can move.
        /// </summary>
        public bool canMove { get { return _canMove && !isDead; } }
        private bool _canMove;

        /// <summary>
        /// Returns whether the character can strafe.
        /// </summary>
        public bool canStrafe { get { return _canStrafe && !isDead; } }
        private bool _canStrafe = true;

        /// <summary>
        /// Returns whether the AcquiringGround action is active, signifying that the character is
        /// landing on the ground. AcquiringGround is added by RPGCharacterMovementController.
        /// </summary>
        public bool acquiringGround { get { return IsActive("AcquiringGround"); } }

        /// <summary>
        /// Returns whether the Death action is active.
        /// </summary>
        public bool isDead { get { return IsActive("Death"); } }

        /// <summary>
        /// Returns whether the Fall action is active. Fall is added by
        /// RPGCharacterMovementController.
        /// </summary>
        public bool isFalling { get { return IsActive("Fall"); } }

        /// <summary>
        /// Returns whether the Idle action is active. Idle is added by
        /// RPGCharacterMovementController.
        /// </summary>
        public bool isIdle { get { return IsActive("Idle"); } }

        /// <summary>
        /// Returns whether the Injure action is active.
        /// </summary>
        public bool isInjured { get { return IsActive("Injure"); } }

        /// <summary>
        /// Returns whether the Move action is active. Idle is added by
        /// RPGCharacterMovementController.
        /// </summary>
        public bool isMoving { get { return IsActive("Move"); } }

        /// <summary>
        /// Returns whether the Navigation action is active. Navigation is added by
        /// RPGCharacterNavigationController.
        /// </summary>
        public bool isNavigating { get { return IsActive("Navigation"); } }

        /// <summary>
        /// Returns whether the Roll action is active. Roll is added by
        /// RPGCharacterMovementController.
        /// </summary>
        public bool isRolling { get { return IsActive("DiveRoll"); } }

        /// <summary>
        /// Returns whether the Roll action is active. Roll is added by
        /// RPGCharacterMovementController.
        /// </summary>
        public bool isKnockback { get { return IsActive("Knockback"); } }

        /// <summary>
        /// Returns whether the Strafe action is active.
        /// </summary>
        public bool isStrafing { get { return IsActive("Strafe"); } }

        /// <summary>
        /// Returns whether the MaintainingGround action is active, signifying that the character
        /// is on the ground. MaintainingGround is added by RPGCharacterMovementController. If the
        /// action does not exist, this defaults to true.
        /// </summary>
        public bool maintainingGround {
            get {
                if (HandlerExists("MaintainingGround")) { return IsActive("MaintainingGround"); }
                return true;
            }
        }

        /// <summary>
        /// Vector3 for move input. Use SetMoveInput to change this.
        /// </summary>
        public Vector3 moveInput { get { return _moveInput; } }
        private Vector3 _moveInput;

        /// <summary>
        /// Vector3 for aim input. Use SetAimInput to change this.
        /// </summary>
        public Vector3 aimInput { get { return _aimInput; } }
        private Vector3 _aimInput;

        /// <summary>
        /// Vector3 for jump input. Use SetJumpInput to change this.
        /// </summary>
        public Vector3 jumpInput { get { return _jumpInput; } }
        private Vector3 _jumpInput;

        /// <summary>
        /// Camera relative input in the XZ plane. This is calculated when SetMoveInput is called.
        /// </summary>
        public Vector3 cameraRelativeInput { get { return _cameraRelativeInput; } }
        private Vector3 _cameraRelativeInput;

        /// <summary>
        /// Integer weapon number for the right hand. See the Weapon enum in AnimationData.cs for a
        /// full list.
        /// </summary>
        [HideInInspector] public int rightWeapon = 0;

        /// <summary>
        /// Integer weapon number for the left hand. See the Weapon enum in AnimationData.cs for a
        /// full list.
        /// </summary>
        [HideInInspector] public int leftWeapon = 0;

		/// <summary>
		/// Returns whether a weapon is held in the right hand. This is false if the character is
		/// unarmed or relaxed.
		public bool hasRightWeapon { get { return AnimationData.IsRightWeapon(rightWeapon); } }

        /// <summary>
        /// Returns whether a weapon is held in the left hand. This is false if the character is
        /// unarmed or relaxed.
        /// </summary>
        public bool hasLeftWeapon { get { return AnimationData.IsLeftWeapon(leftWeapon); } }

        /// <summary>
        /// Returns whether the character is holding a two-handed weapon. Two-handed weapons are
        /// "held" in the right hand.
        /// </summary>
        public bool hasTwoHandedWeapon { get { return AnimationData.Is2HandedWeapon(rightWeapon); } }

		/// <summary>
		/// Returns whether the character is in Unarmed or Relax state.
		/// </summary>
		public bool hasNoWeapon { get { return rightWeapon < 1 && leftWeapon < 1; } }

		private Dictionary<string, IActionHandler> actionHandlers = new Dictionary<string, IActionHandler>();

        #region Initialization

        private void Awake()
        {
            // Setup Animator, add AnimationEvents script.
            animator = GetComponentInChildren<Animator>();
            if (animator == null) {
                Debug.LogError("ERROR: THERE IS NO ANIMATOR COMPONENT ON CHILD OF CHARACTER.");
                Debug.Break();
            }
            animator.gameObject.AddComponent<RPGCharacterAnimatorEvents>();
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

            animator.SetInteger("Weapon", 0);
            animator.SetInteger("WeaponSwitch", 0);

			// Setup IKhands if used.
            ikHands = GetComponentInChildren<IKHands>();

            SetHandler("Attack", new Actions.Attack());
            SetHandler("Death", new Actions.SimpleActionHandler(Death, Revive));
            SetHandler("Injure", new Actions.SimpleActionHandler(StartInjured, EndInjured));
            SetHandler("Null", new Actions.Null());
            SetHandler("SlowTime", new Actions.SlowTime());
            SetHandler("Strafe", new Actions.SimpleActionHandler(StartStrafe, EndStrafe));

            // Unlock actions and movement.
            Unlock(true, true);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Set an action handler.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        /// <param name="handler">The handler associated with this action.</param>
        public void SetHandler(string action, IActionHandler handler)
        {
            actionHandlers[action] = handler;
        }

        /// <summary>
        /// Get an action handler by name. If it doesn't exist, return the Null handler.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        public IActionHandler GetHandler(string action)
        {
            if (HandlerExists(action)) { return actionHandlers[action]; }
            Debug.LogError("RPGCharacterController: No handler for action \"" + action + "\"");
            return actionHandlers["Null"];
        }

        /// <summary>
        /// Check if a handler exists.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        /// <returns>Whether or not that action exists on this controller.</returns>
        public bool HandlerExists(string action)
        {
            return actionHandlers.ContainsKey(action);
        }

        /// <summary>
        /// Check if an action is active.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        /// <returns>Whether the action is active. If the action does not exist, returns false.</returns>
        public bool IsActive(string action)
        {
            return GetHandler(action).IsActive();
        }

        /// <summary>
        /// Check if an action can be started.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        /// <returns>Whether the action can be started. If the action does not exist, returns false.</returns>
        public bool CanStartAction(string action)
        {
            return GetHandler(action).CanStartAction(this);
        }

        /// <summary>
        /// Check if an action can be ended.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        /// <returns>Whether the action can be ended. If the action does not exist, returns false.</returns>
        public bool CanEndAction(string action)
        {
            return GetHandler(action).CanEndAction(this);
        }

        /// <summary>
        /// Start the action with the specified context. If the action does not exist, there is no effect.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        /// <param name="context">Contextual object used by this action. Leave blank if none is required.</param>
        public void StartAction(string action, object context = null)
        {
            GetHandler(action).StartAction(this, context);
        }

        /// <summary>
        /// End the action. If the action does not exist, there is no effect.
        /// </summary>
        /// <param name="action">Name of the action.</param>
        public void EndAction(string action)
        {
            GetHandler(action).EndAction(this);
        }

        #endregion

        #region Updates

        private void LateUpdate()
        {
            // Update Animator animation speed.
            animator.SetFloat("AnimationSpeed", animationSpeed);
        }

        #endregion

        #region Input Axes

        /// <summary>
        /// Set move input. This method expects the x-axis to be left-right input and the
        /// y-axis to be up-down input.
        ///
        /// The z-axis is ignored, but the type is a Vector3 in case you wish to use the z-axis.
        ///
        /// This method computes CameraRelativeInput using the x and y axis of the move input
        /// and the main camera, producing a normalized Vector3 in the XZ plane.
        /// </summary>
        /// <param name="_moveInput">Vector3 move input</param>
        public void SetMoveInput(Vector3 _moveInput)
        {
            this._moveInput = _moveInput;

            // Forward vector relative to the camera along the x-z plane.
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            // Right vector relative to the camera always orthogonal to the forward vector.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 relativeVelocity = _moveInput.x * right + _moveInput.y * forward;

            // Reduce input for diagonal movement.
            if (relativeVelocity.magnitude > 1) { relativeVelocity.Normalize(); }

            _cameraRelativeInput = relativeVelocity;
        }

        /// <summary>
        /// Set aim input. This is a position in world space of the object that the character
        /// is aiming at, so that you can easily lock on to a moving target.
        /// </summary>
        /// <param name="_aimInput">Vector3 aim input.</param>
        public void SetAimInput(Vector3 _aimInput)
        {
            this._aimInput = _aimInput;
        }

        /// <summary>
        /// Set jump input. Use this with Vector3.up and Vector3.down (y-axis).
        ///
        /// The X and Z axes are  ignored, but the type is a Vector3 in case you wish to
        /// use the X and Z axes for other actions.
        /// </summary>
        /// <param name="_jumpInput">Vector3 jump input.</param>
        public void SetJumpInput(Vector3 _jumpInput)
        {
            this._jumpInput = _jumpInput;
        }

        #endregion

        #region Movement

        /// <summary>
        /// Dive Roll.
        ///
        /// Use the "DiveRoll" action for a friendly interface.
        /// </summary>
        /// <param name="rollNumber">1- Forward.</param>
        public void DiveRoll(int rollNumber)
        {
            animator.SetInteger("Action", 1);
            SetAnimatorTrigger(AnimatorTrigger.DiveRollTrigger);
            Lock(true, true, true, 0, 1f);
        }

        /// <summary>
        /// Knockback in the specified direction.
        ///
        /// Use the "Knockback" action for a friendly interface. Forwards only for Unarmed state.
        /// </summary>
        /// <param name="direction">1- Backwards.</param>
        public void Knockback(int direction)
        {
            animator.SetInteger("Action", direction);
            SetAnimatorTrigger(AnimatorTrigger.KnockbackTrigger);
            Lock(true, true, true, 0, 1f);
        }

        #endregion

        #region Combat

        /// <summary>
        /// Trigger an attack animation.
        ///
        /// Use the "Attack" action for a friendly interface.
        /// </summary>
        /// <param name="attackNumber">Animation number to play. See AnimationData.RandomAttackNumber for details.</param>
        /// <param name="leftWeapon">Left side weapon. See Weapon enum in AnimationData.cs.</param>
        /// <param name="rightWeapon">Right-hand weapon. See Weapon enum in AnimationData.cs.</param>
        /// <param name="duration">Duration in seconds that animation is locked.</param>
        public void Attack(int attackNumber, int leftWeapon, int rightWeapon, float duration)
        {
            Lock(true, true, true, 0, duration);

			// Trigger the animation.
			animator.SetInteger("Action", attackNumber);
            SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
        }

        /// <summary>
        /// Trigger the running attack animation.
        ///
        /// Use the "Attack" action for a friendly interface.
        /// </summary>
        /// <param name="attackSide">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
        /// <param name="leftWeapon">Whether to attack on the left side.</param>
        /// <param name="rightWeapon">Whether to attack on the right side.</param>
        /// <param name="dualWeapon">Whether to attack on both sides.</param>
        /// <param name="twoHandedWeapon">If wielding a two-handed weapon.</param>
        public void RunningAttack(int attackSide, bool leftWeapon, bool rightWeapon, bool twoHandedWeapon)
        {
            if (attackSide == 1 && leftWeapon) {
                animator.SetInteger("Action", 1);
                SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
            } else if (attackSide == 2 && rightWeapon) {
                animator.SetInteger("Action", 4);
                SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
            } else if (twoHandedWeapon) {
                animator.SetInteger("Action", 1);
                SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
            }
        }

        /// <summary>
        /// Strafe left and right while still facing a target.
        ///
        /// Use the "Strafe" action for a friendly interface.
        /// </summary>
        public void StartStrafe()
        {
        }

        /// <summary>
        /// Stop strafing.
        ///
        /// Use the "Strafe" action for a friendly interface.
        /// </summary>
        public void EndStrafe()
        {
        }

        /// <summary>
        /// Get hit with an attack.
        ///
        /// Use the "GetHit" action for a friendly interface.
        /// </summary>
        public void GetHit(int hitNumber)
        {
            animator.SetInteger("Action", hitNumber);
            SetAnimatorTrigger(AnimatorTrigger.GetHitTrigger);
            Lock(true, true, true, 0.1f, 0.4f);
			SetIKPause(0.6f);
		}

        /// <summary>
        /// Fall over unconscious.
        ///
        /// Use the "Death" action for a friendly interface.
        /// </summary>
        public void Death()
        {
            SetAnimatorTrigger(AnimatorTrigger.DeathTrigger);
            Lock(true, true, false, 0.1f, 0f);
			SetIKOff();
        }

        /// <summary>
        /// Regain consciousness.
        ///
        /// Use the "Death" action for a friendly interface.
        /// </summary>
        public void Revive()
        {
            SetAnimatorTrigger(AnimatorTrigger.ReviveTrigger);
            Lock(true, true, true, 0f, 1f);
			SetIKPause(1f);
        }

        #endregion

        #region Emotes

        /// <summary>
        /// Switch to the injured state.
        ///
        /// Use the "Injure" action for a friendly interface.
        /// </summary>
        public void StartInjured()
        {
            animator.SetBool("Injured", true);
        }

        /// <summary>
        /// Recover from the injured state.
        ///
        /// Use the "Injure" action for a friendly interface.
        /// </summary>
        public void EndInjured()
        {
            animator.SetBool("Injured", false);
        }

        #endregion

        #region Misc

        /// <summary>
        /// Gets the object with the animator on it. Useful if that object is a child of this one.
        /// </summary>
        /// <returns>GameObject to which the animator is attacked.</returns>
        public GameObject GetAnimatorTarget()
        {
            return animator.gameObject;
        }

        private IEnumerator _GetCurrentAnimationLength()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log(animator.GetCurrentAnimatorClipInfo(0).Length);
        }

        /// <summary>
        /// Lock character movement and/or action, on a delay for a set time.
        /// </summary>
        /// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
        /// <param name="lockAction">If set to <c>true</c> lock action.</param>
        /// <param name="timed">If set to <c>true</c> timed.</param>
        /// <param name="delayTime">Delay time.</param>
        /// <param name="lockTime">Lock time.</param>
        public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
        {
            StopCoroutine("_Lock");
            StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
        }

        //Timed -1 = infinite, 0 = no, 1 = yes.
        private IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
        {
            if (delayTime > 0) { yield return new WaitForSeconds(delayTime); }
            if (lockMovement) {
                _canMove = false;
                OnLockMovement();
            }
            if (lockAction) {
                _canAction = false;
                OnLockActions();
            }
            if (timed) {
                if (lockTime > 0) { yield return new WaitForSeconds(lockTime); }
                Unlock(lockMovement, lockAction);
            }
        }

        /// <summary>
        /// Let character move and act again.
        /// </summary>
        /// <param name="movement">Unlock movement if true.</param>
        /// <param name="actions">Unlock actions if true.</param>
        public void Unlock(bool movement, bool actions)
        {
            if (movement) {
                _canMove = true;
                OnUnlockMovement();
            }
            if (actions) {
                _canAction = true;
                OnUnlockActions();
            }
        }

		public void SetIKOff()
		{
			if (ikHands != null) {
				ikHands.leftHandPositionWeight = 0;
				ikHands.leftHandRotationWeight = 0;
			}
		}

		public void SetIKOn()
		{
			if (ikHands != null) {
				ikHands.leftHandPositionWeight = 1;
				ikHands.leftHandRotationWeight = 1;
			}
		}

		public void SetIKPause(float pauseTime)
		{
			if (ikHands != null && ikHands.isUsed) {
				ikHands.SetIKPause(pauseTime);
			}
		}

		/// <summary>
		/// Set Animator Trigger.
		/// </summary>
		public void SetAnimatorTrigger(AnimatorTrigger trigger)
        {
            //Debug.Log("SetAnimatorTrigger: " + trigger + " - " + (int)trigger);
            animator.SetInteger("TriggerNumber", (int)trigger);
            animator.SetTrigger("Trigger");
        }

        /// <summary>
        /// Set Animator Trigger using legacy Animation Trigger names.
        /// </summary>
        public void LegacySetAnimationTrigger(string trigger)
        {
            //Debug.Log("LegacyAnimationTrigger: " + (AnimatorTrigger)System.Enum.Parse(typeof(AnimatorTrigger), trigger) + " - " + (int)(AnimatorTrigger)System.Enum.Parse(typeof(AnimatorTrigger), trigger));
            AnimatorTrigger parsed_enum = (AnimatorTrigger)System.Enum.Parse(typeof(AnimatorTrigger), trigger);
            animator.SetInteger("TriggerNumber", (int)(AnimatorTrigger)System.Enum.Parse(typeof(AnimatorTrigger), trigger));
            animator.SetTrigger("Trigger");
        }

        /// <summary>
        /// Log out current animator settings.
        /// </summary>
        public void AnimatorDebug()
        {
            Debug.Log("ANIMATOR SETTINGS---------------------------");
			Debug.Log("AnimationSpeed: " + animator.GetFloat("AnimationSpeed"));
			Debug.Log("Velocity X: " + animator.GetFloat("Velocity X"));
            Debug.Log("Velocity Z: " + animator.GetFloat("Velocity Z"));
            Debug.Log("Moving: " + animator.GetBool("Moving"));
            Debug.Log("Injured: " + animator.GetBool("Injured"));
            Debug.Log("Weapon: " + animator.GetInteger("Weapon"));
            Debug.Log("WeaponSwitch: " + animator.GetInteger("WeaponSwitch"));
            Debug.Log("LeftRight: " + animator.GetInteger("LeftRight"));
            Debug.Log("LeftWeapon: " + animator.GetInteger("LeftWeapon"));
            Debug.Log("RightWeapon: " + animator.GetInteger("RightWeapon"));
            Debug.Log("Jumping: " + animator.GetInteger("Jumping"));
            Debug.Log("Action: " + animator.GetInteger("Action"));
			Debug.Log("TriggerNumber: " + animator.GetInteger("TriggerNumber"));
		}

        /// <summary>
        /// Log out current controller settings.
        /// </summary>
        public void ControllerDebug()
        {
            Debug.Log("CONTROLLER SETTINGS---------------------------");
            Debug.Log("AnimationSpeed: " + animationSpeed);
            Debug.Log("canAction: " + canAction);
            Debug.Log("canMove: " + canMove);
            Debug.Log("canStrafe: " + canStrafe);
            Debug.Log("acquiringGround: " + acquiringGround);
            Debug.Log("maintainingGround: " + maintainingGround);
            Debug.Log("isDead: " + isDead);
            Debug.Log("isFalling: " + isFalling);
            Debug.Log("isIdle: " + isIdle);
            Debug.Log("isInjured: " + isInjured);
            Debug.Log("Aiming: " + animator.GetBool("Aiming"));
            Debug.Log("isMoving: " + isMoving);
            Debug.Log("isNavigating: " + isNavigating);
            Debug.Log("isRolling: " + isRolling);
            Debug.Log("isKnockback: " + isKnockback);
            Debug.Log("isStrafing: " + isStrafing);
            Debug.Log("moveInput: " + moveInput);
            Debug.Log("aimInput: " + aimInput);
            Debug.Log("jumpInput: " + jumpInput);
            Debug.Log("cameraRelativeInput: " + cameraRelativeInput);
            Debug.Log("rightWeapon: " + rightWeapon);
            Debug.Log("leftWeapon: " + leftWeapon);
            Debug.Log("hasRightWeapon: " + hasRightWeapon);
            Debug.Log("hasLeftWeapon: " + hasLeftWeapon);
            Debug.Log("hasTwoHandedWeapon: " + hasTwoHandedWeapon);
        }

        #endregion
    }
}