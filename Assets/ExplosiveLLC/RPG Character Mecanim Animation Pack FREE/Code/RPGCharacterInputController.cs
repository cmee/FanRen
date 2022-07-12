using UnityEngine;
using RPGCharacterAnimsFREE.Actions;

namespace RPGCharacterAnimsFREE
{
	[HelpURL("https://docs.unity3d.com/Manual/class-InputManager.html")]

	public class RPGCharacterInputController : MonoBehaviour
    {
        RPGCharacterController rpgCharacterController;

        // Inputs.
        private float inputHorizontal = 0;
        private float inputVertical = 0;
        private bool inputJump;
        private bool inputLightHit;
        private bool inputDeath;
        private bool inputAttackL;
        private bool inputAttackR;
        private float inputSwitchUpDown;
        private float inputAim;
        private bool inputAiming;
        private bool inputRoll;

        // Variables.
        private Vector3 moveInput;
        private bool isJumpHeld;
        private Vector3 currentAim;
        private float inputPauseTimeout = 0;
        private bool inputPaused = false;

        private void Awake()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
            currentAim = Vector3.zero;
        }

        private void Update()
        {
            if (inputPaused) {
                if (Time.time > inputPauseTimeout) {
                    inputPaused = false;
                } else {
                    return;
                }
            }

            Inputs();
            Moving();
            Damage();
            SwitchWeapons();
            Strafing();
            Rolling();
            Attacking();
        }

        /// <summary>
        /// Pause input for a number of seconds.
        /// </summary>
        /// <param name="timeout">The amount of time in seconds to ignore input</param>
        public void PauseInput(float timeout)
        {
            inputPaused = true;
            inputPauseTimeout = Time.time + timeout;
        }

        /// <summary>
        /// Input abstraction for easier asset updates using outside control schemes.
        /// </summary>
        private void Inputs()
        {
            try {
                inputJump = Input.GetButtonDown("Jump");
                isJumpHeld = Input.GetButton("Jump");
                inputLightHit = Input.GetButtonDown("LightHit");
                inputDeath = Input.GetButtonDown("Death");
                inputAttackL = Input.GetButtonDown("AttackL");
                inputAttackR = Input.GetButtonDown("AttackR");
                inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
                inputAim = Input.GetAxisRaw("Aim");
                inputAiming = Input.GetButton("Aiming");
                inputHorizontal = Input.GetAxisRaw("Horizontal");
                inputVertical = Input.GetAxisRaw("Vertical");
                inputRoll = Input.GetButtonDown("L3");

                // Injury toggle.
                if (Input.GetKeyDown(KeyCode.I)) {
                    if (rpgCharacterController.CanStartAction("Injure")) {
                        rpgCharacterController.StartAction("Injure");
                    } else if (rpgCharacterController.CanEndAction("Injure")) {
                        rpgCharacterController.EndAction("Injure");
                    }
                }
                // Slow time toggle.
                if (Input.GetKeyDown(KeyCode.T)) {
                    if (rpgCharacterController.CanStartAction("SlowTime")) {
                        rpgCharacterController.StartAction("SlowTime", 0.125f);
                    } else if (rpgCharacterController.CanEndAction("SlowTime")) {
                        rpgCharacterController.EndAction("SlowTime");
                    }
                }
                // Pause toggle.
                if (Input.GetKeyDown(KeyCode.P)) {
                    if (rpgCharacterController.CanStartAction("SlowTime")) {
                        rpgCharacterController.StartAction("SlowTime", 0f);
                    } else if (rpgCharacterController.CanEndAction("SlowTime")) {
                        rpgCharacterController.EndAction("SlowTime");
                    }
                }
            } catch (System.Exception) { Debug.LogError("Inputs not found!"); }
        }

        public bool HasMoveInput()
        {
            return moveInput != Vector3.zero;
        }

        public bool HasAimInput()
        {
            return inputAiming || inputAim < -0.1f;
        }

        public void Moving()
        {
            moveInput = new Vector3(inputHorizontal, inputVertical, 0f);
            rpgCharacterController.SetMoveInput(moveInput);

            // Set the input on the jump axis every frame.
            Vector3 jumpInput = isJumpHeld ? Vector3.up : Vector3.zero;
            rpgCharacterController.SetJumpInput(jumpInput);

            // If we pressed jump button this frame, jump.
            if (inputJump && rpgCharacterController.CanStartAction("Jump")) {
                rpgCharacterController.StartAction("Jump");
            }
        }

        public void Rolling()
        {
            if (!inputRoll) { return; }
            if (!rpgCharacterController.CanStartAction("DiveRoll")) { return; }

            rpgCharacterController.StartAction("DiveRoll", 1);
        }

        private void Strafing()
        {
            if (rpgCharacterController.canStrafe) {
                if (inputAim < -0.1f || inputAiming) {
                    if (rpgCharacterController.CanStartAction("Strafe")) { rpgCharacterController.StartAction("Strafe"); }
                } else {
                    if (rpgCharacterController.CanEndAction("Strafe")) { rpgCharacterController.EndAction("Strafe"); }
                }
            }
        }

        private void Attacking()
        {
            if (!rpgCharacterController.CanStartAction("Attack")) { return; }
            if (inputAttackL) {
                rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "Left"));
            } else if (inputAttackR) {
                rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "Right"));
            }
        }

        private void Damage()
        {
            // Hit.
            if (inputLightHit) { rpgCharacterController.StartAction("GetHit", new HitContext()); }

            // Death.
            if (inputDeath) {
                if (rpgCharacterController.CanStartAction("Death")) {
                    rpgCharacterController.StartAction("Death");
                } else if (rpgCharacterController.CanEndAction("Death")) {
                    rpgCharacterController.EndAction("Death");
                }
            }
        }

        /// <summary>
        /// Cycle weapons using directional pad input. Up and Down cycle forward and backward through
        /// the list of two handed weapons. Left cycles through the left hand weapons. Right cycles through
        /// the right hand weapons.
        /// </summary>
        private void SwitchWeapons()
        {
            // Bail out if we can't switch weapons.
            if (!rpgCharacterController.CanStartAction("SwitchWeapon")) { return; }

            bool doSwitch = false;
            SwitchWeaponContext context = new SwitchWeaponContext();
            int weaponNumber = 0;

            // Cycle through 2H weapons any input happens on the up-down axis.
            if (Mathf.Abs(inputSwitchUpDown) > 0.1f) {
                int[] twoHandedWeapons = new int[] {
                    (int) Weapon.TwoHandSword,
                };
                // If we're not wielding 2H weapon already, just switch to the first one in the list.
                if (System.Array.IndexOf(twoHandedWeapons, rpgCharacterController.rightWeapon) == -1) {
					weaponNumber = twoHandedWeapons[0];
				}
                else {
                    weaponNumber = 0;
                }
                // Set up the context and flag that we actually want to perform the switch.
                doSwitch = true;
                context.type = "Switch";
                context.side = "None";
                context.leftWeapon = -1;
                context.rightWeapon = weaponNumber;
            }

            // If we've received input, then "doSwitch" is true, and the context is filled out,
            // so start the SwitchWeapon action.
            if (doSwitch) { rpgCharacterController.StartAction("SwitchWeapon", context); }
        }
    }
}