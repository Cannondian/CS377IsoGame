using RPGCharacterAnims.Extensions;
using RPGCharacterAnims.Lookups;
using UnityEngine;

namespace RPGCharacterAnims.Actions
{
	
	public class Skill:BaseActionHandler<SkillContext>
	{

		public override bool CanStartAction(RPGCharacterController controller)
		{
			//Debug.Log(controller.isRelaxed);
			//Debug.Log(active);
			//Debug.Log(controller.isCasting);
			return !controller.isRelaxed && !active && !controller.isCasting &&
			       controller.canAction && CharacterEnergy.Instance.skillIsReady; }

		public override bool CanEndAction(RPGCharacterController controller)
		{ return active; }

		protected override void _StartAction(RPGCharacterController controller, SkillContext context)
		{
			var position = context.pos;
			var type = context.type;
			var terrain = context.terrainType;
			var duration = 0f;
			
			
			
			/*
			if (attackNumber == -1) {
				switch (context.type) {
					case "Attack":
						attackNumber = AnimationData.RandomAttackNumber(attackSide, weaponNumber);
						break;
					case "Special":
						attackNumber = 1;
						break;
				}
			}*/

			duration = 0.5f;
			
		/*	if (controller.isMoving) {
				controller.MovingSkill(
					attackSide,
					controller.hasLeftWeapon,
					controller.hasRightWeapon,
					controller.hasDualWeapons,
					controller.hasTwoHandedWeapon
				);
				EndAction(controller);
			}
			else if (context.type == "Kick") {
				controller.AttackKick(attackNumber);
				EndAction(controller);
			}*/
			if (context.type == "Skill") {
				
				
				
				
				EventBus.TriggerEvent(EventTypes.Events.ON_PARTICLE_FX_FOR_SKILL, 
					new EventTypes.SkillUsedParam(Color.green, position, FXList.FXlist.ArtilleryStrike, 0.3f, 2.5f));
				controller.Skill(Characters.Jisa,
					0.1f,
					terrain);
					
				
				
						
				Debug.Log("it gets here");
						
				EndAction(controller);
			}
			/*else if (context.type == "Special") {
				controller.isSpecial = true;
				controller.StartSpecial(attackNumber);
			}*/
		}

		protected override void _EndAction(RPGCharacterController controller)
		{
			if (controller.isUsingSkill) {
				
				//controller.EndSkill();
			}
		}
	}
}
