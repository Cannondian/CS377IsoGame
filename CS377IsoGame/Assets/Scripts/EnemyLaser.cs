using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private bool TouchingPlayer;
    public float DamagePerTick; // Should be set by whoever is calling this script

    void Start()
    {
        TouchingPlayer = false;
    }

    void FixedUpdate()
    {
        if (TouchingPlayer)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, DamagePerTick);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TouchingPlayer = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TouchingPlayer = false;
        }
    }
}
