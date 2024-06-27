using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroMeleeSwordCollider : MonoBehaviour
{
    public Collider attackCollider;
    private float delayTime = 0f;
    private float AttackDamage;
    private EnemyAI myAI;


    private void Awake()
    {
        EnemyAI enemyAI = GetComponentInParent<EnemyAI>(); // Searches up the hierarchy
        myAI = enemyAI;
        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI component not found in any parent GameObjects.");
            return;
        }
        AttackDamage = enemyAI.AttackDamage;
        print("MeleeAgro thrust damage:" + AttackDamage);

    }
    // This method sets the delay time and starts the delayed invocation
    public void ActivateWithDelay(float t)
    {
        delayTime = t;
        Invoke(nameof(ActivateObject), delayTime);
    }

    // This method is the actual activation logic
    private void ActivateObject()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player HIT with sword!");
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, AttackDamage);
            EnemyAI.CallPlayerHit(myAI);
        }
    }
}
