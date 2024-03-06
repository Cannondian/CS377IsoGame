using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollManager : Singleton<RollManager>
{
    public bool roll1Ready;
    public bool roll2Ready;
    public float roll1ReadyIn;
    public float roll2ReadyIn;
    public float rollCooldown;
    public bool invulnerable;
    
    // Start is called before the first frame update
    void Start()
    {
        roll1Ready = true;
        roll2Ready = true;
        rollCooldown = 5;
        roll1ReadyIn = rollCooldown;
        roll2ReadyIn = rollCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!roll1Ready)
        {
            if (roll1ReadyIn <= 0)
            {
                roll1Ready = true;
                roll1ReadyIn = rollCooldown;
            }
            else
            {
                roll1ReadyIn -= Time.deltaTime;
            }
        }

        if (!roll2Ready)
        {
            if (roll2ReadyIn <= 0)
            {
                roll2Ready = true;
                roll2ReadyIn = rollCooldown;
            }
            else
            {
                roll2ReadyIn -= Time.deltaTime;
            }
        }
    }

    public void Roll()
    {
        if (roll1Ready)
        {
            roll1Ready = false;
            StartCoroutine(Invulnerability());

        }
        else if (roll2Ready)
        {
            roll2Ready = false;
            StartCoroutine(Invulnerability());
        }
    }

    public bool CanRoll()
    {
        if (roll1Ready || roll2Ready)
        {
            return true;
        }

        return false;
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        yield return new WaitForSeconds(0.5f);
        invulnerable = false;
    }
}
