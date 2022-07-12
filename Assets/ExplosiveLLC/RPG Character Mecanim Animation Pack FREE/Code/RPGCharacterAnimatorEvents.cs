using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimatorMoveEvent : UnityEvent<Vector3, Quaternion> { }

namespace RPGCharacterAnimsFREE
{
    public class RPGCharacterAnimatorEvents : MonoBehaviour
    {
		// Placeholder functions for Animation events.
		public UnityEvent OnHit = new UnityEvent();
        public UnityEvent OnShoot = new UnityEvent();
        public UnityEvent OnFootR = new UnityEvent();
        public UnityEvent OnFootL = new UnityEvent();
        public UnityEvent OnLand = new UnityEvent();
        public UnityEvent OnWeaponSwitch = new UnityEvent();
        public AnimatorMoveEvent OnMove = new AnimatorMoveEvent();

		// Components.
		RPGCharacterController rpgCharacterController;
        Animator animator;

        void Awake()
        {
			rpgCharacterController = GetComponentInParent<RPGCharacterController>();
            animator = GetComponent<Animator>();
        }

        public void Hit()
        {
            OnHit.Invoke();
        }

        public void Shoot()
        {
            OnShoot.Invoke();
        }

        public void FootR()
        {
            OnFootR.Invoke();
        }

        public void FootL()
        {
            OnFootL.Invoke();
        }

        public void Land()
        {
            OnLand.Invoke();
        }

        public void WeaponSwitch()
        {
            OnWeaponSwitch.Invoke();
        }

		// Used for animations that contain root motion to drive the character’s 
		// position and rotation using the “Motion” node of the animation file.
		void OnAnimatorMove()
        {
            if (animator) {
				if (!rpgCharacterController.isNavigating) { OnMove.Invoke(animator.deltaPosition, animator.rootRotation); }
            }
        }
    }
}