namespace RPGCharacterAnimsFREE.Actions
{
    public class Fall : MovementActionHandler<EmptyContext>
    {
        public Fall(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !controller.maintainingGround;
        }

        protected override void _StartAction(RPGCharacterController controller, EmptyContext context)
        {
            movement.currentState = RPGCharacterState.Fall;
        }

        public override bool IsActive()
        {
            return movement.currentState != null && (RPGCharacterState)movement.currentState == RPGCharacterState.Fall;
        }
    }
}