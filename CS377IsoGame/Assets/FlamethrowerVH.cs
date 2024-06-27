using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlamethrowerVH: MonoBehaviour {
    private float tickRate;
    private bool firstTick;
    private float startTime;
    private List<Collider> hitOnCurrentTick = new List<Collider>();
   
    
    // Start is called before the first frame update
    void Start()
    {
        firstTick = true;
        tickRate = 0.2f;

    }

    private void OnEnable()
    {
        if (TileMastery.Instance.VelheretMod2)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_VH_FLAMETHROWER_ENABLED, true);
        }
    }

    private void OnDisable()
    {
        if (TileMastery.Instance.VelheretMod2)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_VH_FLAMETHROWER_ENABLED, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (firstTick)
        {
            startTime = Time.time;
            firstTick = false;
        }

        if (Time.time - startTime >= tickRate)
        {
            startTime += tickRate * 2;
            hitOnCurrentTick.Clear();
        }

        if (!hitOnCurrentTick.Contains(other))
        {
            hitOnCurrentTick.Add(other);
            var damage = DamageCalculator.Instance.PlayerFlamethrowerTickVH();
            DamageEnemy(other, damage);
            other.GetComponent<ConditionState>().SetCondition(StatusConditions.statusList.Poisoned, 6, 
                1.3f + TileMastery.Instance.masteryOverVelheret / 30);
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 2, Damage.Types.Poison));
        }
        
    }
    void DamageEnemy(Collider other, float damage)
    {
        var enemyStats = other.gameObject.GetComponent<StatsTemplate>();
        if (enemyStats.amIEnemy)
        {
            enemyStats.TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
