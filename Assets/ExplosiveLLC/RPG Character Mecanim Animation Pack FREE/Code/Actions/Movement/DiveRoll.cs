namespace RPGCharacterAnimsFREE.Actions
{
    public class DiveRoll : MovementActionHandler<int>
    {
        public DiveRoll(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        {
            return controller.canAction;
        }

        protected override void _StartAction(RPGCharacterController controller, int context)
        {
            controller.DiveRoll(context);
            movement.currentState = RPGCharacterState.DiveRoll;
			controller.SetIKPause(1.25f);
		}

        public override bool IsActive()
        {
            return movement.currentState != null && (RPGCharacterState)movement.currentState == RPGCharacterState.DiveRoll;
        }
    }
}