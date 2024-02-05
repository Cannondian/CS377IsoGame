using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierMissile : MonoBehaviour
{
    private Rigidbody rb;
    private MeshCollider col1;
    private SphereCollider col2;
    private float LaunchTime;
    private CharacterProximityDetector prox;

    // These values should be set by the caller
    public float Damage = 10f;
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
        // Explode if past lifespan or if player proximity detector is triggered
        if (Time.time - LaunchTime > MaxLifeSpan || prox.Collided)
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, Damage);
        }

        // TODO: smarter collision detection with ground. Checking for collision with object in "Walkable" layer
        // creates a bunch of performance issues so this instead should be fine for now
        Explode();
    }

    void Explode()
    {
        // TODO: SoundFX, animations, etc.

        Destroy(gameObject);
    }
}
