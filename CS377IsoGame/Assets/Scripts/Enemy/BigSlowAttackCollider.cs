using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackIndicator : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Collider attackCollider;
    private float AttackDamage;


    private void Awake()
    {
        Transform rootParent = transform.root;
        AttackDamage = rootParent.GetComponent<EnemyAI>().AttackDamage;
        print("Big Slow attack damage:" + AttackDamage);

    }

    // Call this method at the start of the attack animation
    public void ActivateIndicator()
    {
        gameObject.SetActive(true);
        StartCoroutine(ActivateExplosionAndColliderAtEnd());
    }

    private IEnumerator ActivateExplosionAndColliderAtEnd()
    {
        // Assuming the animation takes 2 seconds, adjust this value as needed
        yield return new WaitForSeconds(2f);
        GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        attackCollider.enabled = true;
        Debug.Log("HAPPENED");
        // Optionally, deactivate the indicator and collider after a short duration
        yield return new WaitForSeconds(0.5f); // Adjust based on the desired visibility duration
        gameObject.SetActive(false);
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player HIT by the BIG FAT");
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, AttackDamage);

        }
    }
}
