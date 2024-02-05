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

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<RPGCharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {

            Collider myCollider = collision.GetContact(0).thisCollider;
            int attackNumber = controller.comboIndex;
            if (controller.isAttacking && myCollider == weaponCollider && attackNumber == 3)
            {
                EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                    new EventTypes.Event9Param(collision.contacts[0].point, FXList.FXlist.ElectricHit,
                        myCollider.transform.rotation));
                EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                    CharacterEnergy.Instance.energyFromEnhancedBasic);
                EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);
            }
            else if (controller.isAttacking && (myCollider == weaponCollider || (myCollider == legCollider && attackNumber == 5)))

            {
                EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
                    new EventTypes.Event5Param(attackNumber, 0));
                EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                    new EventTypes.Event9Param(collision.contacts[0].point, FXList.FXlist.BasicHit2,
                        myCollider.transform.rotation));
                ;
            }

        }

    }
    
}