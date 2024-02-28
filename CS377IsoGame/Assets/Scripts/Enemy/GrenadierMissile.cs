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
        Explode();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, Damage);
        } // We do not explode the grenade here! (Otherwise we'd have grenades exploding mid air)
    }

    void FixedUpdate()
    {
        // For some reason, some collisions are missing. This should ensure it always blows up
        if (transform.position.y <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        Destroy(
            Instantiate(
                ExplosionFX, 
                transform.position, 
                Quaternion.identity
            ),
            1f // Destroy FX after 1 second
        );

        SoundManager.PlaySound(SoundManager.Sound.Generic_Explosion, transform.position, 0.5f);

        Destroy(gameObject);
    }
}
