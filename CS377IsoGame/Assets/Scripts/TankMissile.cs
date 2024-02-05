using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TankMissile : MonoBehaviour
{
    public float MaxLifeSpan = 5f;
    public float MissileDamage = 10f;

    private Rigidbody rb;
    private float SpawnTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnTime = Time.time;
    }

    void Update()
    {
        // Rotate missile to velocity direction
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }

    void FixedUpdate()
    {
        if (Time.time - SpawnTime > MaxLifeSpan) 
        {
            // Do not explode but simply delete
            // (we don't want any extra effects from missiles that go far off)
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Explode();
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, MissileDamage);
        }

        else if (col.gameObject.tag == "Collide")
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
