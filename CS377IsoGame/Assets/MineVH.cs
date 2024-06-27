using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using UnityEngine;

public class MineVH: MonoBehaviour {
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
        var controller = other.gameObject.GetComponent<RPGCharacterController>();
        if (!hitThisCast.Contains(other.gameObject))
        {
            if (controller != null && !TileMastery.Instance.VelheretMod1) {
                other.GetComponent<ConditionState>().SetCondition(StatusConditions.statusList.Rejuvenation, 
                4  + TileMastery.Instance.masteryOverVelheret / 20, 1+TileMastery.Instance.masteryOverVelheret);
                
            }
            
            hitThisCast.Add(other.gameObject);
            var damage = DamageCalculator.Instance.PlayerMineSkillVH();
            DamageEnemy(other, damage);
            isDamageDealt = true;
            if (TileMastery.Instance.VelheretMod1)
            {
                other.GetComponent<ConditionState>().SetCondition(StatusConditions.statusList.Poisoned, 6, 
                    30 + TileMastery.Instance.masteryOverVelheret / 3);
            }
        }
        
    }
    void DamageEnemy(Collider other, float damage)
    {
        var enemyStats = other.gameObject.GetComponent<StatsTemplate>();
        if (enemyStats.amIEnemy)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 4, Damage.Types.Poison));
            enemyStats.TakeDamage(damage);
        }
    }
}
