namespace RPGCharacterAnimsFREE.Actions
{
    public class SwitchWeaponContext
    {
        public string type;
        public string side;

        // "back" or "hips".
        public string sheathLocation;

        public int rightWeapon;
        public int leftWeapon;

        public SwitchWeaponContext()
        {
            this.type = "Instant";
            this.side = "None";
            this.sheathLocation = "Back";
            this.rightWeapon = (int)Weapon.Unarmed;
            this.leftWeapon = (int)Weapon.Unarmed;
        }

        public SwitchWeaponContext(string type, string side, string sheathLocation = "Back", int rightWeapon = -1, int leftWeapon = -1)
        {
            this.type = type;
            this.side = side;
            this.sheathLocation = sheathLocation;
            this.rightWeapon = rightWeapon;
            this.leftWeapon = leftWeapon;
        }

        public void LowercaseStrings()
        {
            type = type.ToLower();
            side = side.ToLower();
            sheathLocation = sheathLocation.ToLower();
        }
    }

    public class SwitchWeapon : BaseActionHandler<SwitchWeaponContext>
    {
        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !IsActive();
        }

        public override bool CanEndAction(RPGCharacterController controller)
        {
            return IsActive();
        }

        protected override void _StartAction(RPGCharacterController controller, SwitchWeaponContext context)
        {
            RPGCharacterWeaponController weaponController = controller.GetComponent<RPGCharacterWeaponController>();
            if (weaponController == null) {
                EndAction(controller);
                return;
            }

            context.LowercaseStrings();

            bool changeRight = false;
            bool sheathRight = false;
            bool unsheathRight = false;
            int fromRight = controller.rightWeapon;
            int toRight = context.rightWeapon;

            int fromLeft = controller.leftWeapon;
            int toLeft = context.leftWeapon;

            bool dualWielding = AnimationData.Is1HandedWeapon(fromRight) && AnimationData.Is1HandedWeapon(fromLeft);
            bool dualUnsheath = context.side == "dual";
            bool dualSheath = false;

            int toAnimatorWeapon = 0;

            // Filter which side is changing.
            switch (context.side) {
                case "none":
                case "right":
                    changeRight = true;
                    if (AnimationData.Is2HandedWeapon(toRight) && !AnimationData.IsNoWeapon(fromLeft)) {
                        toLeft = (int)Weapon.Unarmed;
                        dualSheath = dualWielding;
                    }
                    break;
                case "dual":
                    changeRight = true;
                    dualSheath = dualWielding;
                    break;
            }

            // Force unarmed if sheathing weapons.
            if (context.type == "sheath") {
                if (context.side == "none" || context.side == "right" || context.side == "dual" || context.side == "both") {
                    toRight = (int)Weapon.Unarmed;
                }
            }

            // Sheath weapons first if our starting weapon is different from our desired weapon and we're
            // not starting from an unarmed position.
            if (context.type == "sheath" || context.type == "switch") {
                sheathRight = changeRight && fromRight != toRight && !AnimationData.IsNoWeapon(fromRight);
                toAnimatorWeapon = AnimationData.ConvertToAnimatorWeapon(toLeft, toRight);
            }

            // Unsheath a weapon if our starting weapon is different from our desired weapon and we're
            // not ending on an unarmed position.
            if (context.type == "unsheath" || context.type == "switch") {
                unsheathRight = changeRight && fromRight != toRight && !AnimationData.IsNoWeapon(toRight);
            }

            ///
            /// Actually make changes to the weapon controller.
            ///

            if (context.type == "instant") {
                if (changeRight) { weaponController.InstantWeaponSwitch(toRight); }
            } else {
                // Sheath weapons first if that's necessary.
                if (sheathRight) {
                    // Debug.Log("Sheath Right (dual: " + dualSheath + "): " + fromRight + " > " + toRight);
                    weaponController.SheathWeapon(fromRight, toAnimatorWeapon, dualSheath);
                }
                // Finally, unsheath the desired weapons!
                if (unsheathRight) {
                    // Debug.Log("Unsheath Right (dual: " + dualUnsheath + "): " + toRight);
                    weaponController.UnsheathWeapon(toRight, dualUnsheath);
                }
            }

            // This callback will update the weapons in character controller after all other
            // coroutines finish.
            weaponController.AddCallback(() => {
                if (changeRight) { controller.rightWeapon = toRight; }

                // Turn off the isWeaponSwitching flag and sync weapon object visibility.
                weaponController.SyncWeaponVisibility();
                EndAction(controller);
            });
        }

        protected override void _EndAction(RPGCharacterController controller)
        {
        }
    }
}