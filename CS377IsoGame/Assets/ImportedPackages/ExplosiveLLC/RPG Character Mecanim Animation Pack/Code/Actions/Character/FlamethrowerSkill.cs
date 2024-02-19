using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace RPGCharacterAnims.Actions
{
    
    public class FlamethrowerSkill : BaseActionHandler<FlamethrowerSkillContext>
    {
        public UnityAction<bool> OnUltimateReadyListener;

        private bool isFuelTankEmpty;
        
        public FlamethrowerSkill()
        {
           
            
            
        }
        
        
        
        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !controller.isRelaxed && !active && !controller.isCasting && controller.canAction;
        }

        public override bool CanEndAction(RPGCharacterController controller)
        {
            return active;
        }

        protected override void _StartAction(RPGCharacterController controller, FlamethrowerSkillContext context)
        {
            /*if (!isUltimateReady)
            {
                EndAction(controller);
                return;
            }*/

            var playerTransform = context.playerTransform;
            var terrain = context.terrain;

           
                    EventBus.TriggerEvent(EventTypes.Events.ON_FLAMETHROWER_SKILL_START, 
                        new EventTypes.FlamethrowerStartFXParam(Color.green, playerTransform, context.direction));
                    controller.FlamethrowerSKill();
                    
                    Debug.Log("flamethrower");
                    
            

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
           
            /*else if (context.type == "Special") {
                controller.isSpecial = true;
                controller.StartSpecial(attackNumber);
            }*/
        }

        private void IsUltimateReadyUpdate(bool state)
        {
            isFuelTankEmpty = state;
        }
        
        protected override void _EndAction(RPGCharacterController controller)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_FLAMETHROWER_SKILL_END, FXList.FXlist.Flamethrower);
            controller.StopFlamethrowerSkill();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
           
        }
    }
}
