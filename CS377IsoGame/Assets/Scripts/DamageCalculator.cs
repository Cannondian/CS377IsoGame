using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class DamageCalculator : Singleton<DamageCalculator>
{
    public StatsTemplate playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType(typeof(RPGCharacterController)).GameObject().GetComponent<StatsTemplate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType(typeof(RPGCharacterController)).GameObject().GetComponent<StatsTemplate>();
        }
    }

    public float PlayerShockStickCombo1()
    {
        
        Random random = new Random();
        return (int)(playerStats.tAttack * 0.25f)  + random.Next(-1, 1);
    }

    public float PlayerShockStickCombo2()
    {
        Random random = new Random();
        return (int)(playerStats.tAttack * 0.30f) + random.Next(-1, 1);
    }

    public float PlayerShockStickCombo3()
    {
        Random random = new Random();
        return (int)(playerStats.tAttack * 0.45f) + random.Next(-2, 2);
    }

    public float PlayerMineSkill()
    {
        Random random = new Random();
        return playerStats.tAttack + random.Next((int)(playerStats.tAttack* -0.1), (int)(playerStats.tAttack* 0.1));
    }

    public float PlayerMineSkillIlsihre()
    {
        Random random = new Random();
        return playerStats.tAttack * (1.2f + TileMastery.Instance.masteryOverIlsihre / 100 ) + random.Next((int)(playerStats.tAttack* -0.3), (int)(playerStats.tAttack* 0.3));
    }
    
    public float PlayerMineSkillVH()
    {
        Random random = new Random();
        return playerStats.tAttack * (1.2f + TileMastery.Instance.masteryOverIlsihre / 200 ) + random.Next((int)(playerStats.tAttack* -0.4), (int)(playerStats.tAttack* 0.4));
    }
    public float PlayerMineSkillSH()
    {
        Random random = new Random();
        return (int)(playerStats.tAttack * 0.8f * (1 + TileMastery.Instance._masteryOverShalharan / 200 ) + random.Next((int)(playerStats.tAttack* -0.2), (int)(playerStats.tAttack* 0.2)));
    }
    public float PlayerFlamethrowerTick()
    {
        Random random = new Random();
        return (int)(playerStats.tAttack / 7) + random.Next((int)(playerStats.tAttack / 7 * -0.15), (int)(playerStats.tAttack / 7 * -0.15));
    }
    
    public float PlayerFlamethrowerTickIL()
    {
        Random random = new Random();
        return (int)(((playerStats.tAttack / 7) + random.Next((int)(playerStats.tAttack / 5 * -0.15), (int)(playerStats.tAttack / 5 * -0.15))) 
            * (1 + TileMastery.Instance.masteryOverIlsihre / 150) * Mathf.Pow(1 + ((playerStats.ceHP - playerStats.myCurrentHealth) / playerStats.ceHP) / 2, 3 ));
    }
    public float PlayerFlamethrowerTickVH()
    {
        Random random = new Random();
        return (int)(playerStats.tAttack / 17) + random.Next((int)(playerStats.tAttack / 4 * -0.15), (int)(playerStats.tAttack / 3 * 0.15));
    }

    public float PlayerFlamethrowerTickSH()
    {
        Random random = new Random();
        return (int)((playerStats.tAttack / 7) +
                     random.Next((int)(playerStats.tAttack / 4 * -0.15), (int)(playerStats.tAttack / 3 * 0.15)) *
                     (1 + TileMastery.Instance._masteryOverShalharan / 100));
    }

    public float PlayerInnerFireExplosion()
    {
        int damage = 10 + (int) (playerStats.tAttack / 2 * (1 + TileMastery.Instance.masteryOverIlsihre / 100) *
                     (1 + (playerStats.ceHP - playerStats.myCurrentHealth) / (playerStats.ceHP * 1.7f)));
        return damage;
    }

    public float VHBacklashDamage()
    {
        float damage = 2 + TileMastery.Instance.masteryOverVelheret / 25;
        return damage;
    }
}
