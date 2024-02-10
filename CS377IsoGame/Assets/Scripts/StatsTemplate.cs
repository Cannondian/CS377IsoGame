using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTemplate : MonoBehaviour
{
    #region statsList

    public enum statsList
    {
        HP,
        Speed,
        Attack,
        Defense,
        AttackSpeed,
        Resistance,
        RRR,
        Talent
        
    }

    #endregion
    
    
    #region baseStats

    public float baseHP;
    public float baseSpeed;
    public float baseAttack;
    public float baseDefense;
    public float baseAttackSpeed;
    public float baseResistance;
    public float baseRRR; //resource regeneration rate
    public float baseTalent;
    
    #endregion
    
    #region CurrentEffectiveStats
    
    public float ceHP;
    public float ceSpeed;
    public float ceAttack;
    public float ceDefense;
    public float ceAttackSpeed;
    public float ceResistance;
    public float ceRRR; //resource regeneration rate
    public float ceTalent;
    
    #endregion
    
    
    #region temporaryStats 
    //calculated and stored based on external parameters
    
    public float tHP;
    public float tSpeed;
    public float tAttack;
    public float tDefense;
    public float tAttackSpeed;
    public float tResistance;
    public float tRRR; //resource regeneration rate
    public float tTalent;
    
    #endregion
    
    #region statModifiers
    
    public List<StatModifier> HPModifiers;
    public List<StatModifier> speedModifiers;
    public List<StatModifier> attackModifiers;
    public List<StatModifier> defenseModifiers;
    public List<StatModifier> attackSpeedModifiers;
    public List<StatModifier> resistanceModifiers;
    public List<StatModifier> RRRModifiers; //resource regeneration rate
    public List<StatModifier> talentModifiers;
    
   
    
    #endregion


    public StatsTemplate(float hp, float speed, float attack, float defense, float attackSpeed, float resistance, float RRR, float talent)
    {
        baseHP = hp;
        baseSpeed = speed;
        baseAttack = attack;
        baseDefense = defense;
        baseAttackSpeed = attackSpeed;
        baseResistance = resistance;
        baseRRR = RRR;
        baseTalent = talent;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        HPModifiers = new List<StatModifier>();
        speedModifiers = new List<StatModifier>();
        attackModifiers = new List<StatModifier>();
        defenseModifiers = new List<StatModifier>();
        attackSpeedModifiers = new List<StatModifier>();
        resistanceModifiers = new List<StatModifier>();
        RRRModifiers = new List<StatModifier>();
        talentModifiers = new List<StatModifier>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddModifier(statsList stat, StatModifier modifier)
    {
        switch (stat)
        {
            case statsList.HP:
                HPModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Speed:
                speedModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Attack:
                attackModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Defense:
               defenseModifiers.Add(modifier);
               CalculateTemporaryStats();
                break;
            case statsList.AttackSpeed:
                attackSpeedModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Resistance:
                resistanceModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.RRR:
                RRRModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Talent:
                talentModifiers.Add(modifier);
                CalculateTemporaryStats();
                break;
        }
    }

    public void RemoveModifier(statsList stat, StatModifier modifier)
    {
        switch (stat)
        {
            case statsList.HP:
                HPModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Speed:
                speedModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Attack:
                attackModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Defense:
                defenseModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.AttackSpeed:
                attackSpeedModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Resistance:
                resistanceModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.RRR:
                RRRModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
            case statsList.Talent:
                talentModifiers.Remove(modifier);
                CalculateTemporaryStats();
                break;
        }
    }

    
    public void CalculateCES() //calculate and store current effective stat when stats change by sources other than buffs and debuffs
    {
        //the invariant must hold that there should be no temporary modifiers in the list of stat modifiers when this function is called
        
    }

    public void CalculateTemporaryStats() //calculate and store temporary stats whenever buffs/debuffs are applied.
    {
        
    }
    
    public void CalculateBaseStats() //calculate and store base stats whenever base stats change (ex: on level up, permanent buffs etc.)
    {
        
    }
    
    
}
