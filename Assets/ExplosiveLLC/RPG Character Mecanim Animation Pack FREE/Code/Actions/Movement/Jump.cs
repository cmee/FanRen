namespace RPGCharacterAnimsFREE.Actions
{
    public class Jump : MovementActionHandler<EmptyContext>
    {
        public Jump(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        {
            return movement.canJump && controller.maintainingGround && controller.canAction;
        }

        protected override void _StartAction(RPGCharacterController controller, EmptyContext context)
        {
            movement.currentState = RPGCharacterState.Jump;
        }

        public override bool IsActive()
        {
            return movement.currentState != null && (RPGCharacterState)movement.currentState == RPGCharacterState.Jump;
        }
    }
}