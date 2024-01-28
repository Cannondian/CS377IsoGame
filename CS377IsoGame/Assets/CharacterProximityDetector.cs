using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProximityDetector : MonoBehaviour
{
    public bool Collided;

    void Start()
    {
        Collided = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "player")
        {
            Collided = true;
        }
    }
}
