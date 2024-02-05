using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private bool TouchingPlayer;
    public float DamagePerTick = 0.1f;

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
        Debug.Log("ENTER");
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
