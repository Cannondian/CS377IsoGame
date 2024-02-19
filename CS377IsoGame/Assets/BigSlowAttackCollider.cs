using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackIndicator : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Collider attackCollider;

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

        // Optionally, deactivate the indicator and collider after a short duration
        yield return new WaitForSeconds(0.5f); // Adjust based on the desired visibility duration
        gameObject.SetActive(false);
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player HIT");
        }
    }
}
