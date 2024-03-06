using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFlamethrowerCollisions : MonoBehaviour
{
    private float tickRate;
    private bool firstTick;
    private float startTime;
    private List<Collider> hitOnCurrentTick = new List<Collider>();
    
    // Start is called before the first frame update
    void Start()
    {
        firstTick = true;
        tickRate = 0.25f;
        
    }

    private void OnTriggerStay(Collider other)
    {
        PushObject(other);
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
            var damage = DamageCalculator.Instance.PlayerFlamethrowerTickSH();
            DamageEnemy(other, damage);
            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                new EventTypes.FloatingDamageParam(other.gameObject, damage, 1));
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
    
    void PushObject(Collider other)
    {
        Transform pushedTransform = other.gameObject.transform;
        Vector3 pushDirection = (pushedTransform.position - transform.position).normalized;
        pushDirection.y = 0;
        RaycastHit info;
        
        if (Physics.SphereCast(pushedTransform.position + new Vector3(0, 3, 0), 
                Mathf.Max(other.bounds.extents.x, other.bounds.extents.z), pushDirection, out info, 0.1f, 1 << 11))
        {
            
            

        }
        else
        {
            pushedTransform.position = Vector3.Lerp(pushedTransform.position, pushedTransform.position + pushDirection * 3 , 0.05f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
