using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;

public class WindMineCollisions : MonoBehaviour
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
        RPGCharacterController controller = other.gameObject.GetComponent<RPGCharacterController>();
        if (!hitThisCast.Contains(other.gameObject) && controller == null)
        {
            hitThisCast.Add(other.gameObject);
            var damage = DamageCalculator.Instance.PlayerMineSkillSH();
            DamageEnemy(other, damage);
            isDamageDealt = true;
            PushObject(other);
        }
        
    }

    void PushObject(Collider other)
    {
        Transform pushedTransform = other.gameObject.transform;
        Vector3 pushDirection = (pushedTransform.position - transform.position).normalized;
        pushDirection.y = 0;
        RaycastHit info;
        if (Physics.Raycast(transform.position + new Vector3(0, 3, 0), pushDirection, out info, 2f, 1 << 11))
        {
            pushedTransform.position =
                pushedTransform.position + (info.distance * pushDirection - other.bounds.extents);
        }
        else
        {
            pushedTransform.position = pushedTransform.position + pushDirection * 2;
        }
    }
    void DamageEnemy(Collider other, float damage)
    {
        var enemyStats = other.gameObject.GetComponent<StatsTemplate>();
        if (enemyStats != null && enemyStats.amIEnemy)
        {
            enemyStats.TakeDamage(damage);
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 4));
        }
    }
}
