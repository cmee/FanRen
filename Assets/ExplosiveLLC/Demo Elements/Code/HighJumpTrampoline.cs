using UnityEngine;

namespace RPGCharacterAnimsFREE
{
    public class HighJumpTrampoline : MonoBehaviour
    {
        GameObject character;
        float oldJumpSpeed;

        void Update()
        {
            if (character != null) {
                RPGCharacterController controller = character.GetComponent<RPGCharacterController>();
                controller.SetJumpInput(Vector3.up);
                if (controller.CanStartAction("Jump")) { controller.StartAction("Jump"); }
            }
        }

        private void OnTriggerEnter(Collider collide)
        {
            RPGCharacterController controller = collide.gameObject.GetComponent<RPGCharacterController>();

            if (controller != null) {
                character = collide.gameObject;

                RPGCharacterMovementController movement = character.GetComponent<RPGCharacterMovementController>();
                oldJumpSpeed = movement.jumpSpeed;
                movement.jumpSpeed = oldJumpSpeed * 2f;
				Debug.Log("Trampoline!");
			}
        }

        private void OnTriggerExit(Collider collide)
        {
            if (collide.gameObject == character) {
                RPGCharacterMovementController movement = character.GetComponent<RPGCharacterMovementController>();
                movement.jumpSpeed = oldJumpSpeed;
                character = null;
            }
        }
    }
}