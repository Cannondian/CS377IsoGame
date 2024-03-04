using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DamageCalculator : Singleton<DamageCalculator>
{
    public StatsTemplate playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public float PlayerFlamethrowerTick()
    {
        Random random = new Random();
        return (int)(playerStats.tAttack / 7) + random.Next((int)(playerStats.tAttack / 7 * -0.1), (int)(playerStats.tAttack / 7 * -0.1));
    }
    
    public float PlayerFlamethrowerTickIL()
    {
        Random random = new Random();
        return (int)((playerStats.tAttack / 5) + random.Next((int)(playerStats.tAttack / 7 * -0.1), (int)(playerStats.tAttack / 7 * -0.1))) 
            * (1 + TileMastery.Instance.masteryOverIlsihre / 150) * Mathf.Pow(1 + ((playerStats.ceHP - playerStats.myCurrentHealth) / playerStats.ceHP) / 3, 2 );
    }

    public float PlayerInnerFireExplosion()
    {
        return playerStats.tAttack / 2 * (1 + TileMastery.Instance.masteryOverIlsihre / 100) *
                (1 + (playerStats.ceHP - playerStats.myCurrentHealth) / (playerStats.ceHP * 2));
    }
}
