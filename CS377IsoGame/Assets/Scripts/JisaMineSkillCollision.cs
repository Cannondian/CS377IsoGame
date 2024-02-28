using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JisaMineSkillCollision : MonoBehaviour
{
    private bool isDamageDealt;
    private BoxCollider myCollider;
    private List<GameObject> hitThisCast = new List<GameObject>();
    
   //ensure that damage is dealt only once
   private void Start()
   {
       myCollider = GetComponent<BoxCollider>();
       hitThisCast.Clear();
       StartCoroutine(DisableCollider());

   }

   // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1);
        myCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitThisCast.Contains(other.gameObject))
        {
            hitThisCast.Add(other.gameObject);
            var damage = DamageCalculator.Instance.PlayerMineSkill();
            DamageEnemy(other, damage);
            isDamageDealt = true;
        }
        
    }
    void DamageEnemy(Collider other, float damage)
    {
        var enemyStats = other.gameObject.GetComponent<StatsTemplate>();
        if (enemyStats.amIEnemy)
        {
            enemyStats.TakeDamage(damage);
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                                  new EventTypes.FloatingDamageParam(other.gameObject, damage, 4));
        }
    }
}
