using UnityEngine;
using RPGCharacterAnimsFREE.Actions;

namespace RPGCharacterAnimsFREE
{
    public class GUIControls : MonoBehaviour
    {
        private RPGCharacterController rpgCharacterController;
        private RPGCharacterWeaponController rpgCharacterWeaponController;
        private bool useInstant;
        private bool instantToggle;
        private bool useNavigation;
        private Vector3 jumpInput;
        public GameObject nav;

        private void Start()
        {
            // Get other RPG Character components.
            rpgCharacterController = GetComponent<RPGCharacterController>();
            rpgCharacterWeaponController = GetComponent<RPGCharacterWeaponController>();
        }

        private void OnGUI()
        {
            // Character is not dead.
            if (!rpgCharacterController.isDead) {

				// Character is on the ground.
				if (rpgCharacterController.maintainingGround) {
					Navigation();
					WeaponSwitching();
					Attacks();
					Damage();
					RollDodgeTurn();
				}
				Jumping();
            }
            Misc();
        }

        private void Navigation()
        {
            useNavigation = GUI.Toggle(new Rect(610, 105, 100, 30), useNavigation, "Navigation");

            if (!rpgCharacterController.HandlerExists("Navigation")) {
                return;
            }
            if (useNavigation) {
                nav.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                nav.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                    nav.transform.position = hit.point;
                    if (Input.GetMouseButtonDown(0)) {
                        rpgCharacterController.StartAction("Navigation", hit.point);
                    }
                }
            } else {
                if (rpgCharacterController.CanEndAction("Navigation")) {
                    nav.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    nav.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    rpgCharacterController.EndAction("Navigation");
                }
            }
        }

        private void Attacks()
        {
            if (!rpgCharacterController.CanStartAction("Attack")) {
                return;
            }
            if (rpgCharacterController.leftWeapon == (int)Weapon.Unarmed) {
                if (GUI.Button(new Rect(25, 85, 100, 30), "Attack L")) {
                    rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "Left"));
                }
            }
            if (rpgCharacterController.rightWeapon == (int)Weapon.Unarmed) {
                if (GUI.Button(new Rect(130, 85, 100, 30), "Attack R")) {
                    rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "Right"));
                }
            }
            if (rpgCharacterController.hasTwoHandedWeapon) {
                if (GUI.Button(new Rect(130, 85, 100, 30), "Attack")) {
                    rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "None"));
                }
            }
        }

        private void Damage()
        {
            if (GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")) { rpgCharacterController.StartAction("GetHit", new HitContext()); }
            if (GUI.Button(new Rect(130, 240, 100, 30), "Knockback1")) { rpgCharacterController.StartAction("Knockback", new HitContext(1, Vector3.back)); }
        }

        private void RollDodgeTurn()
        {
            if (rpgCharacterController.CanStartAction("DiveRoll")) {
                if (GUI.Button(new Rect(25, 30, 100, 30), "Dive Roll")) { rpgCharacterController.StartAction("DiveRoll", 1); }
            }
        }

        private void Jumping()
        {
            if (rpgCharacterController.CanStartAction("Jump")) {
                if (GUI.Button(new Rect(25, 175, 100, 30), "Jump")) {
                    rpgCharacterController.SetJumpInput(Vector3.up);
                    rpgCharacterController.StartAction("Jump");
                }
            }
        }

        private void WeaponSwitching()
        {
            if (!rpgCharacterController.CanStartAction("SwitchWeapon")) { return; }

            bool doSwitch = false;
            SwitchWeaponContext context = new SwitchWeaponContext();

            if (rpgCharacterController.rightWeapon != (int)Weapon.Unarmed || rpgCharacterController.leftWeapon != (int)Weapon.Unarmed) {
                if (GUI.Button(new Rect(1115, 280, 100, 30), "Unarmed")) {
                    doSwitch = true;
                    context.type = "Switch";
                    context.side = "Dual";
                    context.leftWeapon = (int)Weapon.Unarmed;
                    context.rightWeapon = (int)Weapon.Unarmed;
                }
            }

            // Two-handed weapons.
            Weapon[] weapons = new Weapon[] {
                Weapon.TwoHandSword,
            };
            int offset = 310;

            foreach (Weapon weapon in weapons) {
                if (rpgCharacterController.rightWeapon != (int)weapon) {
                    string label = weapon.ToString();
                    if (label.StartsWith("TwoHand")) {
                        label = label.Replace("TwoHand", "2H ");
                    }
                    if (GUI.Button(new Rect(1115, offset, 100, 30), label)) {
                        doSwitch = true;
                        context.type = "Switch";
                        context.side = "None";
                        context.leftWeapon = -1;
                        context.rightWeapon = (int)weapon;
                    }
                }
                offset += 30;
            }

            // Instant weapon toggle.
            useInstant = GUI.Toggle(new Rect(1130, 350, 100, 30), useInstant, "Instant");
            if (useInstant) { context.type = "Instant"; }

            // Perform the weapon switch.
            if (doSwitch) { rpgCharacterController.StartAction("SwitchWeapon", context); }
        }

        // Death / Debug.
        private void Misc()
        {
            string deathReviveLabel = rpgCharacterController.isDead ? "Revive" : "Death";
            if (rpgCharacterController.maintainingGround) {
                if (GUI.Button(new Rect(30, 270, 100, 30), deathReviveLabel)) {
                    if (rpgCharacterController.CanStartAction("Death")) {
                        rpgCharacterController.StartAction("Death");
                    } else if (rpgCharacterController.CanEndAction("Death")) {
                        rpgCharacterController.EndAction("Death");
                    }
                }
            }
            // Debug.
            if (GUI.Button(new Rect(600, 20, 120, 30), "Debug Controller")) { rpgCharacterController.ControllerDebug(); }
            if (GUI.Button(new Rect(600, 50, 120, 30), "Debug Animator")) { rpgCharacterController.AnimatorDebug(); }
        }
    }
}