using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierMissile : MonoBehaviour
{
    [SerializeField] GameObject ExplosionFX;

    // These values should be set by whoever instantiates the prefab
    public float Damage = 10f;

    private Rigidbody rb;
    private float LaunchTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchTime = Time.time;
    }

    void Update()
    {
        // Rotate missile to velocity direction
        // transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, Damage);
        }
        Explode();
    }

    void Explode()
    {
        // TODO: SoundFX, animations, etc.

        Destroy(
            Instantiate(
                ExplosionFX, 
                transform.position, 
                Quaternion.identity
            ),
            1f // Destroy FX after 1 second
        );

        Destroy(gameObject);
    }
}
