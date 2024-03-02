using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TankMissile : MonoBehaviour
{
    [SerializeField] GameObject ExplosionFX;

    private Rigidbody rb;
    private float SpawnTime;

    private Transform player;

    // These values should be set by the caller
    public float MaxLifeSpan = 5f;
    public float Damage = 10f;
    public float HomingStrength = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnTime = Time.time;

        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Rotate missile to velocity direction
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }

    void FixedUpdate()
    {
        Vector3 ToPlayer = (player.position - transform.position).normalized;
        Vector3 Delta = ToPlayer - transform.forward;
        Delta = new Vector3(Delta.x, 0f, Delta.z);
        rb.velocity = rb.velocity + Delta * HomingStrength;

        if (Time.time - SpawnTime > MaxLifeSpan || transform.position.y <= 0) 
        {
            Explode();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Explode();
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, Damage);
        }

        else if (col.gameObject.layer == LayerMask.NameToLayer("Walkable"))
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
