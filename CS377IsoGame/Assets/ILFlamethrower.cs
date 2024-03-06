using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILFlamethrower : MonoBehaviour
{
    // Start is called before the first frame update
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

        if (!hitOnCurrentTick.Contains(other) && other.gameObject.GetComponent<StatsTemplate>() != null)
        {
            hitOnCurrentTick.Add(other);
            var damage = DamageCalculator.Instance.PlayerFlamethrowerTickIL();
            DamageEnemy(other, damage);
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 2, Damage.Types.Fire));
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
