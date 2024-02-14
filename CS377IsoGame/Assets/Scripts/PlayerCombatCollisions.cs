using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatCollisions : MonoBehaviour
{
    
  
        //one idea to make this better would be to turn on their colliders at the end of the attack animation
        private RPGCharacterController controller;
        private int attackNumber;
        private bool isNewAttack;
        private float attackTimer;
        
        
        void Start()
        {
            controller = FindObjectOfType<RPGCharacterController>();
            
            attackNumber = -1;
            attackTimer = 0;



        }

        private void Update()
        {
            
            
        }

        private void OnTriggerEnter(Collider other)
        {
            // This assumes that the script is attached to the object with the weaponCollider and legCollider
            //make this trigger once per collision
            //we want to ensure that collisions only trigger appropriate effects once per attack
            if (controller.isAttacking)
            {
                
                //control section to ensure every attack triggers a single collision with every enemy and collisions only trigger
                //after a certain point in the animation
                
                if (attackNumber == controller.comboIndex)
                {
                    isNewAttack = false;
                }
                else if (attackTimer > 0.1f)
                {
                    isNewAttack = true;
                    attackNumber = controller.comboIndex;
                    

                }

                attackTimer = attackTimer + Time.deltaTime;


                if (isNewAttack)
                {
                    if (attackNumber == 3)
                    {
                        Debug.Log("collided");
                        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                            new EventTypes.Event9Param(transform.position, FXList.FXlist.EnhancedHitFX,
                                transform.rotation));

                        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                            CharacterEnergy.Instance.energyFromEnhancedBasic);

                        EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);

                        DamageEnemy(other, 80f);
                        EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                            new EventTypes.FloatingDamageParam(other.gameObject, 80));
                    }

                    else
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
                            new EventTypes.Event5Param(attackNumber, 0));

                        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                            new EventTypes.Event9Param(transform.position, FXList.FXlist.BasicHitFX,
                                transform.rotation));
                        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                            CharacterEnergy.Instance.energyFromEnhancedBasic);
                            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                                new EventTypes.FloatingDamageParam(other.gameObject, 6));
                        DamageEnemy(other, 6f);
                    }

                    attackTimer = 0;
                }

            }
        }

         
        void DamageEnemy(Collider other, float damage)
        {
            var enemyHealth = other.gameObject.GetComponent<Health>();
            if (enemyHealth.amIEnemy)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
}

