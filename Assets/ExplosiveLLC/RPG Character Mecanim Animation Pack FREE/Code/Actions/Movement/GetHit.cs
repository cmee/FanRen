// Hit from front1 - 1
// Hit from front2 - 2
// Hit from back - 3
// Hit from left - 4
// Hit from right - 5

using UnityEngine;

namespace RPGCharacterAnimsFREE.Actions
{
    public class HitContext
    {
        public int number;
        public Vector3 direction;
        public float force;
        public float variableForce;
        public bool relative;

        public HitContext()
        {
            this.number = -1;
            this.direction = Vector3.zero;
            this.force = 8f;
            this.variableForce = 4f;
            this.relative = true;
        }

        public HitContext(int number, Vector3 direction, float force = 8f, float variableForce = 4f, bool relative = true)
        {
            this.number = number;
            this.direction = direction;
            this.force = force;
            this.variableForce = variableForce;
            this.relative = relative;
        }
    }

    public class GetHit : MovementActionHandler<HitContext>
    {
        public GetHit(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        {
            return true;
        }

        protected override void _StartAction(RPGCharacterController controller, HitContext context)
        {
            int hitNumber = context.number;
            Vector3 direction = context.direction;
            float force = context.force;
            float variableForce = context.variableForce;

            if (hitNumber == -1) {
                hitNumber = AnimationData.RandomHitNumber("Hit");
                direction = AnimationData.HitDirection("Hit", hitNumber);
                direction = controller.transform.rotation * direction;
            } else {
                if (context.relative) { direction = controller.transform.rotation * direction; }
            }

            controller.GetHit(hitNumber);
            movement.KnockbackForce(direction, force, variableForce);
        }
    }
}