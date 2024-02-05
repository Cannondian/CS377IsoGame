using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JisaSkillCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageEnemy(other, 50);
    }
    void DamageEnemy(Collider other, float damage)
    {
        var enemyScript = other.gameObject.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }
}
