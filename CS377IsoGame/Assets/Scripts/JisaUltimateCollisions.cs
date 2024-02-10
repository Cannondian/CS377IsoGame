using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JisaUltimateCollisions : MonoBehaviour
{
    //damage over time, so we want to control the collisions at the rate we'd want them to be.
    private float tickTimer;
    
    //general idea for the ultimate is that it will have a concentration-then burst spread kinda feel.
    //Propose camera movement to make it feel more powerful. A slight zoom, then push back.
    private void Awake()
    {
        tickTimer = Time.time + 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
       //wait until it's time to deal damage, then push the tick timerAhead and keep counting towards it
       if (Time.time - tickTimer > 0.5)
       {
           
           tickTimer++;
           DamageEnemy(other, 17);
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
