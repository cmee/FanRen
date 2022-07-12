using UnityEngine;

namespace RPGCharacterAnimsFREE.Actions
{
    public class Move : MovementActionHandler<EmptyContext>
    {
        public Move(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        {
            return controller.canMove &&
                   controller.moveInput != Vector3.zero &&
                   controller.maintainingGround;
        }

        protected override void _StartAction(RPGCharacterController controller, EmptyContext context)
        {
            movement.currentState = RPGCharacterState.Move;
        }

        public override bool IsActive()
        {
            return movement.currentState != null && (RPGCharacterState)movement.currentState == RPGCharacterState.Move;
        }
    }
}