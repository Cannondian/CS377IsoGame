using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
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
            controller = GetComponent<RPGCharacterController>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Collider myCollider = collision.GetContact(0).thisCollider;
            int attackNumber = controller.comboIndex;

            if (controller.isAttacking && myCollider == weaponCollider && attackNumber == 3)
            {
                EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                    new EventTypes.Event9Param(collision.contacts[0].point, FXList.FXlist.ElectricHit, myCollider.transform.rotation));

                EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                    CharacterEnergy.Instance.energyFromEnhancedBasic);

                EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);

                DamageEnemy(collision, 10f);
            }

            else if (controller.isAttacking && (myCollider == weaponCollider || (myCollider == legCollider && attackNumber == 5)))
            {
                EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
                    new EventTypes.Event5Param(attackNumber, 0));

                EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                    new EventTypes.Event9Param(collision.contacts[0].point, FXList.FXlist.BasicHit2, myCollider.transform.rotation));
                
                DamageEnemy(collision, 10f);
            }
        }

        // TODO: Update this! We could use the event bus, for instance (and/or some other more general function)
        void DamageEnemy(Collision collision, float damage)
        {
            var enemyScript = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyScript != null) 
            {
                enemyScript.TakeDamage(damage);
            }
        }
    }
}