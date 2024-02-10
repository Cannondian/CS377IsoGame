using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JisaSkillCollision : MonoBehaviour
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
            DamageEnemy(other, 100);
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
