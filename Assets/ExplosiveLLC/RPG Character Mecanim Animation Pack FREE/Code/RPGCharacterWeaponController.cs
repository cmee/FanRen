using System.Collections;
using UnityEngine;

namespace RPGCharacterAnimsFREE
{
    public class RPGCharacterWeaponController : MonoBehaviour
    {
        private RPGCharacterController rpgCharacterController;
        private Animator animator;
        private CoroutineQueue coroQueue;

        // Weapon Parameters.
        [HideInInspector] bool isWeaponSwitching = false;

        // Weapon Models.
        public GameObject twoHandSword;

        private void Awake()
        {
            coroQueue = new CoroutineQueue(1, StartCoroutine);
            rpgCharacterController = GetComponent<RPGCharacterController>();
            rpgCharacterController.SetHandler("SwitchWeapon", new Actions.SwitchWeapon());

            // Find the Animator component.
            animator = GetComponentInChildren<Animator>();
            StartCoroutine(_HideAllWeapons(false, false));
        }

        private void Start()
        {
            // Listen for the animator's weapon switch event.
            RPGCharacterAnimatorEvents animatorEvents = animator.gameObject.GetComponent<RPGCharacterAnimatorEvents>();
            animatorEvents.OnWeaponSwitch.AddListener(WeaponSwitch);
        }

        /// <summary>
        /// Add a callback to the coroutine queue to be executed in sequence.
        /// </summary>
        /// <param name="callback">The action to call.</param>
        public void AddCallback(System.Action callback)
        {
            coroQueue.RunCallback(callback);
        }

        /// <summary>
        /// Queue a command to unsheath a weapon.
        /// </summary>
        /// <param name="weaponNumber">Weapon to unsheath.</param>
        /// <param name="dual">Whether to unsheath the same weapon in the other hand.</param>
        public void UnsheathWeapon(int weaponNumber, bool dual)
        {
            coroQueue.Run(_UnSheathWeapon(weaponNumber));
        }

        /// <summary>
        /// Async method to unsheath a weapon.
        /// </summary>
        /// <param name="weaponNumber">Weapon to unsheath.</param>
        /// <returns>IEnumerator for use with StartCoroutine.</returns>
        private IEnumerator _UnSheathWeapon(int weaponNumber)
        {
            Debug.Log("UnsheathWeapon:" + weaponNumber);
            isWeaponSwitching = true;

            // Switching to 2handed weapon.
            if (AnimationData.Is2HandedWeapon(weaponNumber)) {

                // Switching from 2handed weapon.
                if (AnimationData.Is2HandedWeapon(animator.GetInteger("Weapon"))) {
                    DoWeaponSwitch(0, weaponNumber, weaponNumber, -1, false);
                    yield return new WaitForSeconds(0.75f);
                    SetAnimator(weaponNumber, -2, animator.GetInteger("Weapon"), -1, -1);
                } else {
                    DoWeaponSwitch(animator.GetInteger("Weapon"), weaponNumber, weaponNumber, -1, false);
                    yield return new WaitForSeconds(0.75f);
                    SetAnimator(weaponNumber, -2, weaponNumber, -1, -1);
                }
            }
            yield return null;
        }

        /// <summary>
        /// Queue a command to sheath the current weapon and switch to a new one.
        /// </summary>
        /// <param name="fromWeapon">Which weapon to sheath.</param>
        /// <param name="toWeapon">Target weapon if immediately unsheathing new weapon.</param>
        /// <param name="dual">Whether to sheath both weapons at once.</param>
        public void SheathWeapon(int fromWeapon, int toWeapon, bool dual)
        {
            coroQueue.Run(_SheathWeapon(fromWeapon, toWeapon));
        }

        /// <summary>
        /// Async method to sheath the current weapon and switch to a new one.
        /// </summary>
        /// <param name="weaponNumber">Which weapon to sheath.</param>
        /// <param name="weaponTo">Target weapon if immediately unsheathing a new weapon.</param>
        /// <returns>IEnumerator for use with StartCoroutine.</returns>
        public IEnumerator _SheathWeapon(int weaponNumber, int weaponTo)
        {
            Debug.Log("Sheath Weapon:" + weaponNumber + "   Weapon To:" + weaponTo);

            // Reset for animation events.
            isWeaponSwitching = true;

            //Switching to Unarmed or Relaxed.
            if (weaponTo < 1) {

                // Have at least 1 weapon.
                if (rpgCharacterController.rightWeapon != 0 || rpgCharacterController.leftWeapon != 0) {

                    // Sheath 2handed weapon.
                    if (AnimationData.Is2HandedWeapon(weaponNumber)) {
                        DoWeaponSwitch(weaponTo, weaponNumber, animator.GetInteger("Weapon"), -1, true);
                        yield return new WaitForSeconds(0.5f);
                        SetAnimator(weaponTo, -2, 0, 0, -1);
                    }
                }
            }
            // Switching to 2handed weapon.
            else if (AnimationData.Is2HandedWeapon(weaponTo)) {

                DoWeaponSwitch(0, weaponNumber, animator.GetInteger("Weapon"), -1, true);
                yield return new WaitForSeconds(0.5f);
                SetAnimator(weaponNumber, -2, weaponNumber, 0, -1);
            }
            yield return null;
        }

        /// <summary>
        /// Switch to the weapon number instantly.
        /// </summary>
        /// <param name="weaponNumber">Weapon to switch to.</param>
        public void InstantWeaponSwitch(int weaponNumber)
        {
            coroQueue.Run(_InstantWeaponSwitch(weaponNumber));
        }

        /// <summary>
        /// Async method to instant weapon switch.
        /// </summary>
        /// <param name="weaponNumber">Weapon number to switch to.</param>
        /// <returns>IEnumerator for use with StartCoroutine.</returns>
        /// /// <summary>
        public IEnumerator _InstantWeaponSwitch(int weaponNumber)
        {
            Debug.Log("_InstantWeaponSwitch:" + weaponNumber);
            rpgCharacterController.SetAnimatorTrigger(AnimatorTrigger.InstantSwitchTrigger);
			rpgCharacterController.SetIKOff();

            // 2Handed.
            if (AnimationData.Is2HandedWeapon(weaponNumber)) {
                animator.SetInteger("Weapon", weaponNumber);
                rpgCharacterController.rightWeapon = 0;
                rpgCharacterController.leftWeapon = 0;
                animator.SetInteger("LeftWeapon", 0);
                animator.SetInteger("RightWeapon", 0);
                StartCoroutine(_HideAllWeapons(false, false));
                StartCoroutine(_WeaponVisibility(weaponNumber, true, false));
				if (AnimationData.IsIKWeapon(weaponNumber)) { rpgCharacterController.SetIKOn(); }
            }
            // Switching to Unarmed or Relax.
            else {
                animator.SetInteger("Weapon", weaponNumber);
                rpgCharacterController.rightWeapon = 0;
                rpgCharacterController.leftWeapon = 0;
                animator.SetInteger("LeftWeapon", 0);
                animator.SetInteger("RightWeapon", 0);
                animator.SetInteger("LeftRight", 0);
                StartCoroutine(_HideAllWeapons(false, false));
            }
            yield return null;
        }

        private void DoWeaponSwitch(int weaponSwitch, int weaponVisibility, int weaponNumber, int leftRight, bool sheath)
        {
            Debug.Log("DoWeaponSwitch:" + weaponSwitch + "   WeaponVisibility:" + weaponVisibility + "   WeaponNumber:" + weaponNumber + "   LeftRight:" + leftRight + "   Sheath:" + sheath);

            // Lock character for switch unless has moving sheath/unsheath anims.
            if (weaponSwitch < 1) {
                if (AnimationData.Is2HandedWeapon(weaponNumber)) { rpgCharacterController.Lock(true, true, true, 0f, 1f); }
            } else if (AnimationData.Is1HandedWeapon(weaponSwitch)) {
                rpgCharacterController.Lock(true, true, true, 0f, 1f);
            }
            // Set weaponSwitch if applicable.
            if (weaponSwitch != -2) { animator.SetInteger("WeaponSwitch", weaponSwitch); }

            animator.SetInteger("Weapon", weaponNumber);

            // Set leftRight if applicable.
            if (leftRight != -1) { animator.SetInteger("LeftRight", leftRight); }
            // Set animator trigger.
            if (sheath) {
                rpgCharacterController.SetAnimatorTrigger(AnimatorTrigger.WeaponSheathTrigger);
                StartCoroutine(_WeaponVisibility(weaponVisibility, false, false));

                // If using IKHands, trigger IK blend.
                if (rpgCharacterController.ikHands != null) { rpgCharacterController.ikHands.BlendIK(false, 0f, 0.2f); }
            } else {
                rpgCharacterController.SetAnimatorTrigger(AnimatorTrigger.WeaponUnsheathTrigger);
                StartCoroutine(_WeaponVisibility(weaponVisibility, true, false));

                // If using IKHands, trigger IK blend.
                if (rpgCharacterController.ikHands != null) { rpgCharacterController.ikHands.BlendIK(true, 0.75f, 1); }
            }
        }

        /// <summary>
        /// Sets the animator state. This method is very close to the metal, it's recommended that you use
        /// other entry points rather than calling this directly.
        /// </summary>
        /// <param name="weapon">Animator weapon number. Use AnimationData's AnimatorWeapon enum.</param>
        /// <param name="weaponSwitch">Weapon switch.</param>
        /// <param name="Lweapon">Left weapon number. Use AnimationData's Weapon enum.</param>
        /// <param name="Rweapon">Right weapon number. Use AnimationData's Weapon enum.</param>
        /// <param name="weaponSide">Weapon side: 0- None, 1- Left, 2- Right, 3- Dual.</param>
        public void SetAnimator(int weapon, int weaponSwitch, int Lweapon, int Rweapon, int weaponSide)
        {
            Debug.Log("SetAnimator - Weapon:" + weapon + "   Weaponswitch:" + weaponSwitch + "   Lweapon:" + Lweapon + "   Rweapon:" + Rweapon + "   Weaponside:" + weaponSide);

            // Set Weapon if applicable.
            if (weapon != -2) { animator.SetInteger("Weapon", weapon); }

			// Set WeaponSwitch if applicable.
            if (weaponSwitch != -2) { animator.SetInteger("WeaponSwitch", weaponSwitch); }

			// Set left weapon if applicable.
            if (Lweapon != -1) { animator.SetInteger("LeftWeapon", Lweapon); }

			// Set right weapon if applicable.
            if (Rweapon != -1) { animator.SetInteger("RightWeapon", Rweapon); }

			// Set weapon side if applicable.
            if (weaponSide != -1) { animator.SetInteger("LeftRight", weaponSide); }
        }

        /// <summary>
        /// Callback to use with Animator's WeaponSwitch event.
        /// </summary>
        public void WeaponSwitch()
        {
            if (isWeaponSwitching) { isWeaponSwitching = false; }
        }

        /// <summary>
        /// Helper method used by other weapon visibility methods to safely set a weapon object's visibility.
        /// This will work even if the object is not set in the component parameters.
        /// </summary>
        /// <param name="weaponObject">Weapon to update.</param>
        /// <param name="visibility">Visibility status.</param>
        public void SafeSetVisibility(GameObject weaponObject, bool visibility)
        {
            if (weaponObject != null) { weaponObject.SetActive(visibility); }
        }

        /// <summary>
        /// Hide all weapon objects and set the animator and the character controller to the unarmed state.
        /// </summary>
        public void HideAllWeapons()
        {
            StartCoroutine(_HideAllWeapons(false, true));
        }

        /// <summary>
        /// Async method to all weapon objects and set the animator and the character controller to the unarmed state.
        /// </summary>
        /// <param name="timed">Whether to wait until a period of time to hide the weapon.</param>
        /// <param name="resetToUnarmed">Whether to reset the animator and the character controller to the unarmed state.</param>
        /// <returns>IEnumerator for use with StartCoroutine.</returns>
        public IEnumerator _HideAllWeapons(bool timed, bool resetToUnarmed)
        {
            if (timed) {
                while (!isWeaponSwitching) { yield return null; }
            }
            // Reset to Unarmed.
            if (resetToUnarmed) {
                animator.SetInteger("Weapon", 0);
                rpgCharacterController.rightWeapon = (int)Weapon.Unarmed;
                rpgCharacterController.leftWeapon = (int)Weapon.Unarmed;
                StartCoroutine(_WeaponVisibility(rpgCharacterController.leftWeapon, false, true));
                animator.SetInteger("RightWeapon", 0);
                animator.SetInteger("LeftWeapon", 0);
                animator.SetInteger("LeftRight", 0);
            }
            SafeSetVisibility(twoHandSword, false);
        }

        /// <summary>
        /// Set a single weapon's visibility.
        /// </summary>
        /// <param name="weaponNumber">Weapon object to set.</param>
        /// <param name="visible">Whether to set it visible or not.</param>
        /// <param name="dual">Whether to update the same weapon in the other hand as well.</param>
        public IEnumerator _WeaponVisibility(int weaponNumber, bool visible, bool dual)
        {
            Debug.Log("WeaponVisibility:" + weaponNumber + "   Visible:" + visible + "   Dual:" + dual);
            while (isWeaponSwitching) { yield return null; }
            if (weaponNumber == 1) { SafeSetVisibility(twoHandSword, visible); }
            yield return null;
        }

        /// <summary>
        /// Sync weapon object visibility to the current weapons in the RPGCharacterController.
        /// </summary>
        public void SyncWeaponVisibility()
        {
            coroQueue.Run(_SyncWeaponVisibility());
        }

        /// <summary>
        /// Async method to sync weapon object visiblity to the current weapons in RPGCharacterController.
        /// This will wait for weapon switching to finish. If your aim is to force this update, call WeaponSwitch
        /// first. This will stop the _HideAllWeapons and _WeaponVisibility coroutines.
        /// </summary>
        /// <returns>IEnumerator for use with.</returns>
        private IEnumerator _SyncWeaponVisibility()
        {
            while (isWeaponSwitching && !(rpgCharacterController.canAction && rpgCharacterController.canMove)) { yield return null; }

            StopCoroutine("_HideAllWeapons");
            StopCoroutine("_WeaponVisibility");

            SafeSetVisibility(twoHandSword, false);

            switch (rpgCharacterController.rightWeapon) {
                case (int)Weapon.TwoHandSword: SafeSetVisibility(twoHandSword, true); break;
            }
        }
    }
}