using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackIndicator : MonoBehaviour
{
    public Collider droneAttackCollider;
    private float AttackDamage;

    private void Awake()
    {
        EnemyAI enemyAI = GetComponentInParent<EnemyAI>(); // Searches up the hierarchy
        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI component not found in any parent GameObjects.");
            return;
        }
        AttackDamage = enemyAI.AttackDamage;
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
