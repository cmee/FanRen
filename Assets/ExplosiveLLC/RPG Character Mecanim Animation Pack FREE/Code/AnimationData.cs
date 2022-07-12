using UnityEngine;

namespace RPGCharacterAnimsFREE
{
    public enum AttackSide
    {
        None = 0,
        Left = 1,
        Right = 2,
        Dual = 3,
    }

    public enum Weapon
    {
        Unarmed = 0,
        TwoHandSword = 1,
    }

    /// <summary>
    /// Enum to use with the "Weapon" parameter of the animator. To convert from a Weapon number,
    /// use AnimationData.ConvertToAnimatorWeapon.
    ///
    /// Two-handed weapons have a 1:1 relationship with this enum, but all one-handed weapons use
    /// ARMED.
    /// </summary>
    public enum AnimatorWeapon
    {
        UNARMED = 0,
        TWOHANDSWORD = 1,
    }

    /// <summary>
    /// Enum to use with the "TriggerNumber" parameter of the animator. Convert to (int) to set.
    /// </summary>
    public enum AnimatorTrigger
    {
		JumpTrigger = 1,
        AttackTrigger = 4,
		GetHitTrigger = 12,
        WeaponSheathTrigger = 15,
        WeaponUnsheathTrigger = 16,
        IdleTrigger = 18,
        DeathTrigger = 20,
        ReviveTrigger = 21,
        InstantSwitchTrigger = 25,
        KnockbackTrigger = 26,
        DiveRollTrigger = 28
    }

    /// <summary>
    /// Static class which contains hardcoded animation constants and helper functions.
    /// </summary>
    public class AnimationData
    {
        /// <summary>
        /// Converts left and right-hand weapon numbers into the legacy weapon number usable by the
        /// animator's "Weapon" parameter.
        /// </summary>
        /// <param name="leftWeapon">Left-hand weapon.</param>
        /// <param name="rightWeapon">Right-hand weapon.</param>
        public static int ConvertToAnimatorWeapon(int leftWeapon, int rightWeapon)
        {
            if (Is2HandedWeapon(rightWeapon)) {
                return rightWeapon;												// 2-handed weapon.
            } else if (IsNoWeapon(rightWeapon) && IsNoWeapon(leftWeapon)) {
                return rightWeapon;												// Unarmed or Relax.
            } else {
                return rightWeapon;
            }
        }

        public static int ConvertToAnimatorLeftRight(int leftWeapon, int rightWeapon)
        {
            if (Is2HandedWeapon(rightWeapon)) {
                return (int)AttackSide.None;
            } else if (leftWeapon > -1 && rightWeapon > -1) {
                return (int)AttackSide.Dual;
            } else if (leftWeapon > 0 && rightWeapon < 1) {
                return (int)AttackSide.Left;
            } else if (leftWeapon < 1 && rightWeapon > 0) {
                return (int)AttackSide.Right;
            }
            return -1;
        }

        /// <summary>
        /// Returns true if the weapon number is Unarmed or a placeholder for the Relax state (i.e. -1).
        /// </summary>
        /// <param name="weaponNumber">Weapon number to test.</param>
        public static bool IsNoWeapon(int weaponNumber)
        {
            return weaponNumber < 1;
        }

        /// <summary>
        /// Returns true if the weapon number is a weapon held in the left hand.
        /// </summary>
        /// <param name="weaponNumber">Weapon number to test.</param>
        public static bool IsLeftWeapon(int weaponNumber)
        {
			return false;
        }

        /// <summary>
        /// Returns true if the weapon number is a weapon held in the right hand.
        /// </summary>
        /// <param name="weaponNumber">Weapon number to test.</param>
        public static bool IsRightWeapon(int weaponNumber)
        {
			return false;
        }

        /// <summary>
        /// Returns true if the weapon number is a 1-handed weapon.
        /// </summary>
        /// <param name="weaponNumber">Weapon number to test.</param>
        public static bool Is1HandedWeapon(int weaponNumber)
        {
            //weaponNumber 7 = Shield
            //weaponNumber 8 = L Sword
            //weaponNumber 9 = R Sword
            //weaponNumber 10 = L Mace
            //weaponNumber 11 = R Mace
            //weaponNumber 12 = L Dagger
            //weaponNumber 13 = R Dagger
            //weaponNumber 14 = L Item
            //weaponNumber 15 = R Item
            //weaponNumber 16 = L Pistol
            //weaponNumber 17 = R Pistol
            //weaponNumber 19 = Right Spear
            return IsLeftWeapon(weaponNumber) || IsRightWeapon(weaponNumber);
        }

        /// <summary>
        /// Returns true if the weapon number is a 2-handed weapon.
        /// </summary>
        /// <param name="weaponNumber">Weapon number to test.</param>
        public static bool Is2HandedWeapon(int weaponNumber)
        {
			//weaponNumber 1 = 2H Sword
			//weaponNumber 2 = 2H Spear
			//weaponNumber 3 = 2H Axe
			//weaponNumber 4 = 2H Bow
			//weaponNumber 5 = 2H Crossbow
			//weaponNumber 6 = 2H Staff
			//weaponNumber 18 = Rifle
			return weaponNumber == ( int )Weapon.TwoHandSword;
        }

		/// <summary>
		/// Returns true if the weapon number can use IKHands.
		/// </summary>
		/// <param name="weaponNumber">Weapon number to test.</param>
		public static bool IsIKWeapon(int weaponNumber)
		{
			//weaponNumber 1 = 2H Sword
			//weaponNumber 2 = 2H Spear
			//weaponNumber 3 = 2H Axe
			//weaponNumber 5 = 2H Crossbow
			//weaponNumber 18 = Rifle
			return weaponNumber == ( int )Weapon.TwoHandSword;
		}

		/// <summary>
		/// Returns the duration of an attack animation. Use side 0 (none) for two-handed weapons.
		/// </summary>
		/// <param name="attackSide">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
		/// <param name="weaponNumber">Weapon that's attacking.</param>
		/// <param name="attackNumber">Attack animation number.</param>
		/// <returns>Duration in seconds of attack animation.</returns>
		public static float AttackDuration(int attackSide, int weaponNumber, int attackNumber)
        {
            float duration = 1f;

            switch (attackSide) {
                case 0:										// Unspecified (2-Handed Weapons)
                    switch (weaponNumber) {
                        case 1: duration = 1.1f; break;     // 2H Sword
                        default:
                            Debug.LogError("RPG Character: no weapon number " + weaponNumber + " for Side 0");
                            break;
                    }
                    break;

                case 1:										// Left Side
                    switch (weaponNumber) {
                        case 0: duration = 0.75f; break;    // Unarmed  (1-3)
                        default:
                            Debug.LogError("RPG Character: no weapon number " + weaponNumber + " for Side 1 (Left)");
                            break;
                    }
                    break;
                case 2:										// Right Side
                    switch (weaponNumber) {
                        case 0: duration = 0.75f; break;    // Unarmed  (4-6)
                        default:
                            Debug.LogError("RPG Character: no weapon number " + weaponNumber + " for Side 2 (Right)");
                            break;
                    }
                    break;
            }

            return duration;
        }

        /// <summary>
        /// Returns the duration of the weapon sheath animation.
        /// </summary>
        /// <param name="attackSide">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
        /// <param name="weaponNumber">Weapon being sheathed.</param>
        /// <returns>Duration in seconds of sheath animation.</returns>
        public static float SheathDuration(int attackSide, int weaponNumber)
        {
            float duration = 1f;

            if (IsNoWeapon(weaponNumber)) {
                duration = 0f;
            } else if (Is2HandedWeapon(weaponNumber)) {
                duration = 1.2f;
            } else if (attackSide == 3) {
                duration = 1f;
            } else {
                duration = 1.05f;
            }

            return duration;
        }

        /// <summary>
        /// Returns a random attack number usable as the animator's Action parameter.
        /// </summary>
        /// <param name="attackSide">Side of the attack: 0- None, 1- Left, 2- Right, 3- Dual.</param>
        /// <param name="weaponNumber">Weapon attacking.</param>
        /// <returns>Attack animation number.</returns>
        public static int RandomAttackNumber(int attackSide, int weaponNumber)
        {
            int offset = 1;
            int numAttacks = 3;

            switch (attackSide) {
                case 0:									// Unspecified (2-Handed Weapons)
                    switch (weaponNumber) {
                        case 1: numAttacks = 6; break; // 2H Sword     (1-6)
                        default:
                            Debug.LogError("RPG Character: no weapon number " + weaponNumber + " for Side 0");
                            break;
                    }
                    break;

                case 1:									// Left Side
                    switch (weaponNumber) {
                        case 0: break;                  // Unarmed  (1-3)
                        default:
                            Debug.LogError("RPG Character: no weapon number " + weaponNumber + " for Side 1 (Left)");
                            break;
                    }
                    break;
                case 2:												// Right Side
                    switch (weaponNumber) {
                        case 0: offset = 4; break;                  // Unarmed  (4-6)
                        default:
                            Debug.LogError("RPG Character: no weapon number " + weaponNumber + " for Side 2 (Right)");
                            break;
                    }
                    break;
                case 3: break;										// Dual Attacks (1-3)
            }

            return Random.Range(offset, numAttacks + offset);
        }

        /// <summary>
        /// Returns the number of a random kick animation.
        /// </summary>
        /// <param name="attackSide">Side of the kick: 1- Left, 2- Right.</param>
        /// <returns>Kick animation number.</returns>
        public static int RandomKickNumber(int attackSide)
        {
            int offset = 1;
            int numAttacks = 2;

            switch (attackSide) {
                case 1:				 // Left Side Kicks (1-2)
                    break;
                case 2:				// Right Side Kicks (3-4)
                    offset = 3;
                    break;
            }

            return Random.Range(offset, numAttacks + offset);
        }

        /// <summary>
        /// Returns the number of a random spellcasting animation.
        /// </summary>
        /// <param name="castType">Type of cast being performed: ("Cast" | "Buff" | "AOE" | "Summon").</param>
        /// <returns>Cast animation number.</returns>
        public static int RandomCastNumber(string castType)
        {
            int offset = 1;
            int numAttacks = 3;

            switch (castType) {
                case "Cast":			// Regular Casting (1-3)
                    break;
                case "Buff":			// Buffs (1-2)
                    numAttacks = 2;
                    break;
                case "AOE":				// AOE (3-4)
                    offset = 3;
                    numAttacks = 2;
                    break;
                case "Summon":			// Summon (5-6)
                    offset = 5;
                    numAttacks = 2;
                    break;
            }

            return Random.Range(offset, numAttacks + offset);
        }

        /// <summary>
        /// Returns the number of a random conversation animation.
        /// </summary>
        public static int RandomConversationNumber()
        {
            return Random.Range(1, 9);
        }

        /// <summary>
        /// Returns the number of a random hit animation.
        /// </summary>
        /// <param name="hitType">Type of hit taken: ("Hit" | "BlockHit" | "Knockback" | "Knockdown")</param>
        public static int RandomHitNumber(string hitType)
        {
            int hits = 5;

            switch (hitType) {
                case "Hit":
                    hits = 5; // Regular hits (1-5)
                    break;
                case "BlockHit":
                    hits = 2; // Blocked hits (1-2)
                    break;
                case "Knockback":
                    hits = 3; // Knockback hits (1-3)
                    break;
                case "Knockdown":
                    hits = 1; // Knockdown hits (1)
                    break;
            }

            return Random.Range(1, hits + 1);
        }

        /// <summary>
        /// Returns the relative direction of knockback force for a hit animation.
        /// </summary>
        /// <param name="hitType">Type of hit taken: ("Hit" | "BlockHit" | "Knockback" | "Knockdown").</param>
        /// <param name="hitNumber">Hit animation number.</param>
        /// <returns>Direction of hit. (relative to character)</returns>
        public static Vector3 HitDirection(string hitType, int hitNumber)
        {
            switch (hitType) {
                case "Hit":
                    switch (hitNumber) {
                        case 1:
                        case 2:
                            return Vector3.back;
                        case 3:
                            return Vector3.forward;
                        case 4:
                            return Vector3.right;
                        case 5:
                            return Vector3.left;
                    }
                    break;
                case "BlockHit":
                    return Vector3.back;
                case "Knockback":
                    switch (hitNumber) {
                        case 1:
                        case 2:
                            return Vector3.back;
                        case 3:
                            return Vector3.forward;
                    }
                    break;
                case "Knockdown":
                    return Vector3.back;
            }

            return Vector3.back;
        }
    }
}