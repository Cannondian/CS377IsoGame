using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroMeleeSwordCollider : MonoBehaviour
{
    public Collider attackCollider;
    private float delayTime = 0f;

    // This method sets the delay time and starts the delayed invocation
    public void ActivateWithDelay(float t)
    {
        delayTime = t;
        Invoke(nameof(ActivateObject), delayTime);
    }

    // This method is the actual activation logic
    private void ActivateObject()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player HIT with sword!");
        }
    }
}
