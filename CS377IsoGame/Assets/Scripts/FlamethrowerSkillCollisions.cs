using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSkillCollisions : MonoBehaviour
{
    private float tickRate;
    private bool firstTick;
    private float startTime;
    
    // Start is called before the first frame update
    void Start()
    {
        firstTick = true;
        tickRate = 0.3f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (firstTick)
        {
            startTime = Time.time - tickRate;
            firstTick = false;
        }

        if (Time.time - startTime >= tickRate)
        {
            startTime += tickRate * 2;
            var damage = DamageCalculator.Instance.PlayerFlamethrowerTick();
            DamageEnemy(other, damage);
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 1));
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
