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
            Debug.Log("Started!");
        }

        private void OnTriggerEnter(Collider other)
        {
            // This assumes that the script is attached to the object with the weaponCollider and legCollider
            Collider myCollider = GetComponent<Collider>();
            int attackNumber = controller.comboIndex;

            Debug.Log("Enemy hit:" + other.name);
        }
        //    if (controller.isAttacking && myCollider == weaponCollider && attackNumber == 3)
        //    {
        //        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
        //            new EventTypes.Event9Param(other.transform.position, FXList.FXlist.ElectricHit, myCollider.transform.rotation));

        //        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
        //            CharacterEnergy.Instance.energyFromEnhancedBasic);

        //        EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);

        //        DamageEnemy(other, 10f);
        //    }

        //    else if (controller.isAttacking && (myCollider == weaponCollider || (myCollider == legCollider && attackNumber == 5)))
        //    {
        //        EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
        //            new EventTypes.Event5Param(attackNumber, 0));

        //        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
        //            new EventTypes.Event9Param(other.transform.position, FXList.FXlist.BasicHit2, myCollider.transform.rotation));

        //        DamageEnemy(other, 10f);
        //    }
        //}

        //// Updated to use Collider parameter
        //void DamageEnemy(Collider other, float damage)
        //{
        //    var enemyScript = other.GetComponent<EnemyAI>();
        //    if (enemyScript != null)
        //    {
        //        enemyScript.TakeDamage(damage);
        //    }
        //}
    }
}
