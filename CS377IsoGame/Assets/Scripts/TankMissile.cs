using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMissile : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Rotate missile to velocity direction
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Collide")
        {
            Explode();
        }
    }

    void Explode()
    {
        // TODO: SoundFX, animations, etc.

        Destroy(gameObject);
    }
}
