using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JisaMineSkillCollision : MonoBehaviour
{
    private bool isDamageDealt;
    
   //ensure that damage is dealt only once
    private void Awake()
    {
        isDamageDealt = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDamageDealt)
        {
            var damage = DamageCalculator.Instance.PlayerMineSkill();
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 4));
            DamageEnemy(other, damage);
            isDamageDealt = true;
        }
        
    }
    void DamageEnemy(Collider other, float damage)
    {
        var enemyHealth = other.gameObject.GetComponent<Health>();
        if (enemyHealth.amIEnemy)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
