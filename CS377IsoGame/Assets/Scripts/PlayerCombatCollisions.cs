using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatCollisions : MonoBehaviour
{

<<<<<<< Updated upstream
    public MeshCollider weaponCollider;
=======
    public CapsuleCollider weaponCollider;
    public CapsuleCollider legCollider;
>>>>>>> Stashed changes

    private RPGCharacterController controller;
    public GameObject damageArcPrefab;

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
        Debug.Log("I got called");
        Collider myCollider = collision.GetContact(0).thisCollider;
        int attackNumber = controller.comboIndex;
        if (controller.isAttacking && myCollider == weaponCollider && CoreChargeManager.Instance.coreChargeState >= 45)
        {
            
            EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                new EventTypes.Event9Param(collision.contacts[0].point, FXList.FXlist.ElectricHit, quaternion.identity));
            EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN, CharacterEnergy.Instance.energyFromEnhancedBasic);
        }
        else if (controller.isAttacking && (myCollider == weaponCollider || myCollider==legCollider))
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN, new EventTypes.Event5Param(attackNumber, 0));
            EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT, 
                new EventTypes.Event9Param(collision.contacts[0].point, FXList.FXlist.BasicHit, quaternion.identity));
            EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN, CharacterEnergy.Instance.energyFromBasic);
        }

        

    }
    
}
