using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatCollisions : MonoBehaviour
{

    public CapsuleCollider weaponCollider;

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
        if (controller.isAttacking && myCollider == weaponCollider)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN, new EventTypes.Event5Param(attackNumber, 0));
        }

    }
    
}
