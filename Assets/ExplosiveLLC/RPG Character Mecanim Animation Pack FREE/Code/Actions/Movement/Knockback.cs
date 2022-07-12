using UnityEngine;

namespace RPGCharacterAnimsFREE.Actions
{
    public class Knockback : MovementActionHandler<HitContext>
    {
        public Knockback(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !controller.isKnockback;
        }

        protected override void _StartAction(RPGCharacterController controller, HitContext context)
        {
            int hitNumber = context.number;
            Vector3 direction = context.direction;
            float force = context.force;
            float variableForce = context.variableForce;

            if (hitNumber == -1) {
                hitNumber = AnimationData.RandomHitNumber("Knockback");
                direction = AnimationData.HitDirection("Knockback", hitNumber);
                direction = controller.transform.rotation * direction;
            } else {
                if (context.relative) { direction = controller.transform.rotation * direction; }
            }

			controller.SetIKPause(1.25f);
			controller.Knockback(hitNumber);
            movement.KnockbackForce(direction, force, variableForce);
            movement.currentState = RPGCharacterState.Knockback;
        }

        public override bool IsActive()
        {
            return movement.currentState != null && (RPGCharacterState)movement.currentState == RPGCharacterState.Knockback;
        }
    }
}