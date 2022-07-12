using UnityEngine;
using RPGCharacterAnimsFREE.Actions;

namespace RPGCharacterAnimsFREE
{
    public class NoJumpSlime : MonoBehaviour
    {
        RPGCharacterController controller;
        IActionHandler oldJumpHandler;

        private void OnTriggerEnter(Collider collide)
        {
            controller = collide.gameObject.GetComponent<RPGCharacterController>();

            if (controller != null) {
                oldJumpHandler = controller.GetHandler("Jump");
                controller.SetHandler("Jump", new SimpleActionHandler(() => {
                    Debug.Log("Can't jump!");
                    controller.EndAction("Jump");
                }, () => { }));
            }
        }

        private void OnTriggerExit(Collider collide)
        {
            if (collide.gameObject == controller.gameObject) {
                controller.SetHandler("Jump", oldJumpHandler);
                controller = null;
                oldJumpHandler = null;
            }
        }
    }
}