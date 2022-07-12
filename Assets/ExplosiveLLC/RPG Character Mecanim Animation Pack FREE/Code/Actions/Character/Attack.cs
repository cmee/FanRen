namespace RPGCharacterAnimsFREE.Actions
{
    public class AttackContext
    {
        public string type;
        public int side;
        public int number;

        public AttackContext(string type, int side, int number = -1)
        {
            this.type = type;
            this.side = side;
            this.number = number;
        }

        public AttackContext(string type, string side, int number = -1)
        {
            this.type = type;
            this.number = number;
            switch (side.ToLower()) {
                case "none":
                    this.side = (int)AttackSide.None;
                    break;
                case "left":
                    this.side = (int)AttackSide.Left;
                    break;
                case "right":
                    this.side = (int)AttackSide.Right;
                    break;
            }
        }
    }

    public class Attack : BaseActionHandler<AttackContext>
    {
        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !active && controller.canAction;
        }

        public override bool CanEndAction(RPGCharacterController controller)
        {
            return active;
        }

        protected override void _StartAction(RPGCharacterController controller, AttackContext context)
        {
            int attackSide = 0;
            int attackNumber = context.number;
            int weaponNumber = controller.rightWeapon;
            float duration = 0f;

            if (context.side == (int)AttackSide.Right && AnimationData.Is2HandedWeapon(weaponNumber)) { context.side = (int)AttackSide.None; }

            switch (context.side) {
                case (int)AttackSide.None:
                    attackSide = 0;
                    weaponNumber = controller.rightWeapon;
                    break;
                case (int)AttackSide.Left:
                    attackSide = 1;
                    weaponNumber = controller.leftWeapon;
                    break;
                case (int)AttackSide.Right:
                    attackSide = 2;
                    weaponNumber = controller.rightWeapon;
                    break;
            }

            if (attackNumber == -1) {
                switch (context.type) {
                    case "Attack":
                        attackNumber = AnimationData.RandomAttackNumber(attackSide, weaponNumber);
                        break;
                    case "Kick":
                        attackNumber = AnimationData.RandomKickNumber(attackSide);
                        break;
                }
            }

            duration = AnimationData.AttackDuration(attackSide, weaponNumber, attackNumber);

            if (controller.isMoving) {
                controller.RunningAttack(
                    attackSide,
                    controller.hasLeftWeapon,
                    controller.hasRightWeapon,
                    controller.hasTwoHandedWeapon
                );
                EndAction(controller);
            } else if (context.type == "Attack") {
                controller.Attack(
                    attackNumber,
                    controller.leftWeapon,
                    controller.rightWeapon,
                    duration
                );
                EndAction(controller);
            }
        }

        protected override void _EndAction(RPGCharacterController controller)
        {
        }
    }
}