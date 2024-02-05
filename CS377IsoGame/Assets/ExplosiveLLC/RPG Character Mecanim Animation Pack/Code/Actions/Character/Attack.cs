using RPGCharacterAnims.Extensions;
using RPGCharacterAnims.Lookups;
using UnityEngine;

namespace RPGCharacterAnims.Actions
{
	public class Attack:BaseActionHandler<AttackContext>
	{
		
		
		public override bool CanStartAction(RPGCharacterController controller)
		{ return !controller.isRelaxed && !active && !controller.isCasting && controller.canAction; }

		public override bool CanEndAction(RPGCharacterController controller)
		{ return active; }

		protected override void _StartAction(RPGCharacterController controller, AttackContext context)
		{
			var attackSide = Side.None;
			var attackNumber = context.number;
			var weaponNumber = controller.rightWeapon;
			var duration = 0f;

			if (context.Side == Side.Right && weaponNumber.Is2HandedWeapon()) { context.Side = Side.None; }

			switch (context.Side) {
				case Side.None:
					attackSide = context.Side;
					weaponNumber = controller.rightWeapon;
					break;
				case Side.Left:
					attackSide = context.Side;
					weaponNumber = controller.leftWeapon;
					break;
				case Side.Right:
					attackSide = context.Side;
					weaponNumber = controller.rightWeapon;
					break;
				case Side.Dual:
					attackSide = context.Side;
					weaponNumber = controller.rightWeapon;
					break;
			}

			if (attackNumber == -1) {
				switch (context.type) {
					case "Attack":
						attackNumber = AnimationData.RandomAttackNumber(attackSide, weaponNumber);
						break;
					case "Special":
						attackNumber = 1;
						break;
				}
			}

			duration = AnimationData.AttackDuration(attackSide, weaponNumber, attackNumber);

			if (!controller.maintainingGround) {
				controller.AirAttack();
				EndAction(controller);
			}
			/*else if (controller.isMoving) {
				
				controller.RunningAttack(
					attackSide,
					controller.hasLeftWeapon,
					controller.hasRightWeapon,
					controller.hasDualWeapons,
					controller.hasTwoHandedWeapon, 
					duration
				);
				EndAction(controller);
			}*/
			else if (context.type == "Kick") {
				controller.AttackKick(attackNumber);
				EndAction(controller);
			}
			else if (context.type == "Attack") {
				controller.Attack(
					
					attackSide,
					controller.leftWeapon,
					controller.rightWeapon,
					duration
				);
				EndAction(controller);
			}
			else if (context.type == "EnhancedAttack") {
				
				controller.EnhancedAttack();
				EndAction(controller);
			}
		}

		
		protected override void _EndAction(RPGCharacterController controller)
		{
			if (controller.isSpecial) {
				controller.isSpecial = false;
				controller.EndSpecial();
			}
		}
	}
}