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
            OnUltimateReadyListener += IsUltimateReadyUpdate;
            OnEnable();
            
        }
        
        
        
        public override bool CanStartAction(RPGCharacterController controller)
        {
            return !controller.isRelaxed && !active && !controller.isCasting && controller.canAction;
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

            if (CharacterEnergy.Instance.ultimateIsReady)
            {

<<<<<<< Updated upstream
                    EventBus.TriggerEvent(EventTypes.Events.ON_CONTINOUS_PACRTICLE_FX_TRIGGER, 
                        new EventTypes.Event2Param(Color.green, playerTransform, FXList.FXlist.Electricity, 5, 0.3f));
                    controller.Ultimate(
                        Characters.Jisa,
                        0.1f);
                    EndAction(controller);
                    Debug.Log("it gets here33");
                    break;
=======
                switch (terrain)
                {
                    case CustomTerrain.Terrains.Grass:
>>>>>>> Stashed changes

                        EventBus.TriggerEvent(EventTypes.Events.ON_CONTINOUS_PACRTICLE_FX_TRIGGER,
                            new EventTypes.Event2Param(Color.green, playerTransform, FXList.FXlist.Electricity1, 5,
                                0.3f));
                        controller.Ultimate(
                            Characters.Jisa,
                            0.1f);
                        EndAction(controller);
                        Debug.Log("it gets here33");
                        break;

                }
            }

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
            EventBus.StartListening(EventTypes.Events.ON_ULTIMATE_READY, OnUltimateReadyListener);
        }

        private void OnDisable()
        {
            EventBus.StopListening(EventTypes.Events.ON_ULTIMATE_READY, OnUltimateReadyListener);
        }
    }
}
