using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMissile : MonoBehaviour
{
    private Rigidbody rb;
    private MeshCollider col1;
    private SphereCollider col2;
    private float LaunchTime;
    private CharacterProximityDetector prox;

    public float MaxLifeSpan = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col1 = GetComponent<MeshCollider>();
        col2 = GetComponent<SphereCollider>();

        LaunchTime = Time.time;

        prox = transform.GetChild(0).gameObject.GetComponent<CharacterProximityDetector>();
    }

    void Update()
    {
        // Rotate missile to velocity direction
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }

    void FixedUpdate()
    {
        if (Time.time - LaunchTime > MaxLifeSpan || prox.Collided)
        {
            Explode();
        }
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
