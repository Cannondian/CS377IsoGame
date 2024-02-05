using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class WeaponTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started!");
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Enemy hit:" + other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            DamageEnemy(other, 20);
        }
    }

    void DamageEnemy(Collider other, float damage)
    {
        var enemyScript = other.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }
}
