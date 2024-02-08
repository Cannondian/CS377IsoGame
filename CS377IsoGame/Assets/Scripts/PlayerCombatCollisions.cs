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
        void Start()
        {
            controller = FindObjectOfType<RPGCharacterController>();
            
            attackNumber = -1;
            

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

                if (attackNumber == controller.comboIndex)
                {
                    isNewAttack = false;
                }                                               //checks if the collision occurs for a new attack
                else
                {
                    isNewAttack = true;
                    attackNumber = controller.comboIndex;
                }



                if (isNewAttack)
                {
                    if (attackNumber == 3)
                    {
                        Debug.Log("collided");
                        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                            new EventTypes.Event9Param(transform.position, FXList.FXlist.ElectricHit,
                                transform.rotation));

                        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                            CharacterEnergy.Instance.energyFromEnhancedBasic);

                        EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);

                        DamageEnemy(other, 80f);
                    }

                    else
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
                            new EventTypes.Event5Param(attackNumber, 0));

                        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                            new EventTypes.Event9Param(transform.position, FXList.FXlist.BasicHit2,
                                transform.rotation));
                        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                            CharacterEnergy.Instance.energyFromBasic);
                        
                        DamageEnemy(other, 6f);
                    }
                }

            }
        }


        void DamageEnemy(Collider other, float damage)
        {
            var enemyScript = other.gameObject.GetComponent<EnemyAI>();
            
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
}

