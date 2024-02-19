using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackIndicator : MonoBehaviour
{
    public Collider droneAttackCollider;
    private float AttackDamage;

    private void Awake()
    {
        Transform rootParent = transform.root;
        AttackDamage = rootParent.GetComponent<EnemyAI>().AttackDamage;
        print("Drone damage:" + AttackDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Drone HIT Player!");
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, AttackDamage);
        }

    }
}
