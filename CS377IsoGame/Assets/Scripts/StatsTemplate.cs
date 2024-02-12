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
        Evasiveness,
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
    public float baseEvasiveness;
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
    public float ceEvasiveness;
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
    public float tEvasiveness;
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
    public List<StatModifier> evasivenessModifiers;
    public List<StatModifier> talentModifiers;
    
    #endregion

    #region StatModifierTrackers

    private bool HPDirty;
    private bool speedDirty;
    private bool attackDirty;
    private bool defenseDirty;
    private bool attackSpeedDirty;
    private bool resistanceDirty;
    private bool RRRDirty;
    private bool evasivenessDirty;
    private bool talentDirty;
    
    
    #endregion

    public StatsTemplate(float hp, float speed, float attack, float defense, float attackSpeed, float resistance, float RRR, float evasiveness, float talent)
    {
        baseHP = hp;
        baseSpeed = speed;
        baseAttack = attack;
        baseDefense = defense;
        baseAttackSpeed = attackSpeed;
        baseResistance = resistance;
        baseRRR = RRR;
        baseEvasiveness = evasiveness;
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
        evasivenessModifiers = new List<StatModifier>();
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
                CalculateTemporaryStats(statsList.HP);
                break;
            case statsList.Speed:
                speedModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Speed);
                break;
            case statsList.Attack:
                attackModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Attack);
                break;
            case statsList.Defense:
               defenseModifiers.Add(modifier);
               CalculateTemporaryStats(statsList.Defense);
                break;
            case statsList.AttackSpeed:
                attackSpeedModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.AttackSpeed);
                break;
            case statsList.Resistance:
                resistanceModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Resistance);
                break;
            case statsList.RRR:
                RRRModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Resistance);
                break;
            case statsList.Evasiveness:
                evasivenessModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Evasiveness);
                break;
            case statsList.Talent:
                talentModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Talent);
                break;
        }
    }

    public void RemoveModifier(statsList stat, StatModifier modifier)
    {
        switch (stat)
        {
            case statsList.HP:
                HPModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.HP);
                break;
            case statsList.Speed:
                speedModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Speed);
                break;
            case statsList.Attack:
                attackModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Attack);
                break;
            case statsList.Defense:
                defenseModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Defense);
                break;
            case statsList.AttackSpeed:
                attackSpeedModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.AttackSpeed);
                break;
            case statsList.Resistance:
                resistanceModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Resistance);
                break;
            case statsList.RRR:
                RRRModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.RRR);
                break;
            case statsList.Evasiveness:
                evasivenessModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Evasiveness);
                break;
            case statsList.Talent:
                talentModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Talent);
                break;
        }
    }

    
    public void CalculateCES() //calculate and store current effective stat when stats change by sources other than buffs and debuffs
    {
        //the invariant must hold that there should be no temporary modifiers in the list of stat modifiers when this function is called
        
    }

    public void
        CalculateTemporaryStats(
            statsList stat) //calculate and store temporary stats whenever buffs/debuffs are applied.
    {
        float delta = 0;
        switch (stat)
        {
            case statsList.HP:

                foreach (StatModifier mod in HPModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseHP * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tHP = ceHP + delta;
                break;
            case statsList.Speed:

                foreach (StatModifier mod in speedModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseSpeed * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tSpeed = ceSpeed + delta;
                break;
            case statsList.Attack:
                foreach (StatModifier mod in attackModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseAttack * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tAttack = ceAttack + delta;
                break;
            case statsList.Defense:

                foreach (StatModifier mod in defenseModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseDefense * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tDefense = ceDefense + delta;
                break;
            case statsList.AttackSpeed:
                foreach (StatModifier mod in attackSpeedModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseAttackSpeed * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tAttackSpeed = ceAttackSpeed + delta;
                break;
            case statsList.Resistance:
                foreach (StatModifier mod in resistanceModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseResistance * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tResistance = ceResistance + delta;
                break;
            case statsList.RRR:
                foreach (StatModifier mod in RRRModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseRRR * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tRRR = ceRRR + delta;
                break;
            case statsList.Evasiveness:
                foreach (StatModifier mod in evasivenessModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseEvasiveness * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tEvasiveness = ceEvasiveness + delta;
                break;
            case statsList.Talent:
                foreach (StatModifier mod in talentModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += baseTalent * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tTalent = ceTalent + delta;
                break;
        }
    }

    public void CalculateBaseStats() //calculate and store base stats whenever base stats change (ex: on level up, permanent buffs etc.)
    {
        
    }
    
    
}
