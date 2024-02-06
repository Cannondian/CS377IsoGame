using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace RPGCharacterAnims.Actions
{
    
    public class Ultimate : BaseActionHandler<UltimateContext>
    {
        public UnityAction<bool> OnUltimateReadyListener;

        private bool isUltimateReady;
        
        public Ultimate()
        {
           
            
            
        }
        
        
        
        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !controller.isRelaxed && !active && !controller.isCasting && controller.canAction && CharacterEnergy.Instance.ultimateIsReady;
        }

        public override bool CanEndAction(RPGCharacterController controller)
        {
            return active;
        }

        protected override void _StartAction(RPGCharacterController controller, UltimateContext context)
        {
            /*if (!isUltimateReady)
            {
                EndAction(controller);
                return;
            }*/

            var playerTransform = context.location;
            var terrain = context.terrain;

           
                    EventBus.TriggerEvent(EventTypes.Events.ON_CONTINOUS_PACRTICLE_FX_TRIGGER, 
                        new EventTypes.Event2Param(Color.green, playerTransform, FXList.FXlist.Electricity1, 10, 0.3f));
                    controller.Ultimate(
                        Characters.Jisa,
                        0.1f);
                    EndAction(controller);
                    Debug.Log("it gets here33");
                    
            

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
            isUltimateReady = state;
        }
        
        protected override void _EndAction(RPGCharacterController controller)
        {
            
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
           
        }
    }
}
