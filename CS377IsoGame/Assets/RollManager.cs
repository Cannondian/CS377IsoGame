using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class RollManager : Singleton<RollManager>
{
    public bool roll1Ready;
    public bool roll2Ready;
    public float roll1ReadyIn;
    public float roll2ReadyIn;
    public float rollCooldown;
    public bool invulnerable;
    public UnityAction<bool> ResetListener;
    public GameObject RollHealthShader;
    
    // Start is called before the first frame update
    void Start()
    {
        roll1Ready = true;
        roll2Ready = true;
        rollCooldown = 5;
        ResetListener += ResetRolls;
        EventBus.StartListening(EventTypes.Events.ON_BUFF_ORB_PICKED_UP, ResetListener);
        //the coroutine is here to ensure that RollCD is updates even if start functions are called in the wrong order
        StartCoroutine(RescaleRollCD());
        
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
        RollHealthShader.SetActive(true);
        
        yield return new WaitForSeconds(0.9f);
        invulnerable = false;
        RollHealthShader.SetActive(false);
    }

    private void ResetRolls(bool ph)
    {
        roll1Ready = true;
        roll2Ready = true;
        roll1ReadyIn = rollCooldown;
        roll2ReadyIn = rollCooldown;
    }

    private IEnumerator RescaleRollCD()
    {
        yield return new WaitForSeconds(1);
        if (TileMastery.Instance._ShalharanMod2) {
            rollCooldown = 5 - TileMastery.Instance.ShalharanEffectIntensity() / 5;
            roll1ReadyIn = rollCooldown; 
            roll2ReadyIn = rollCooldown;
        }
    }
    
}
