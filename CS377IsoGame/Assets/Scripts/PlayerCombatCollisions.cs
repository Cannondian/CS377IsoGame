using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

namespace EgeCode
{
    public class PlayerCombatCollisions : MonoBehaviour
    {
        public CapsuleCollider weaponCollider;
        public CapsuleCollider legCollider;

        private RPGCharacterController controller;

        void Start()
        {
            controller = FindObjectOfType<RPGCharacterController>();
            Debug.Log("Started!");
        }

        private void OnTriggerEnter(Collider other)
        {
            // This assumes that the script is attached to the object with the weaponCollider and legCollider
            //make this trigger once per collision
            int attackNumber = controller.comboIndex;
            
        
            if (controller.isAttacking && attackNumber == 3)
            {
                Debug.Log("collided");
                EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                    new EventTypes.Event9Param(transform.position, FXList.FXlist.ElectricHit, transform.rotation));

                EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN, CharacterEnergy.Instance.energyFromEnhancedBasic);

                EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);

                DamageEnemy(other, 30f);
            }

            else if (controller.isAttacking)
            {
                EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
                   new EventTypes.Event5Param(attackNumber, 0));

                EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                    new EventTypes.Event9Param(transform.position, FXList.FXlist.BasicHit2, transform.rotation));
                EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN, CharacterEnergy.Instance.energyFromBasic);

                DamageEnemy(other, 10f);
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
}
