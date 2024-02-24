using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsTemplate : MonoBehaviour
{
    public static StatsTemplate PlayerStatsToSave;

    #region MyRegion

    [Serializable] public class StatUpdate: UnityEvent<float>
    { }

    public StatUpdate SpeedUpdate;
    public StatUpdate AttackSpeedUpdate;
    public StatUpdate AttackUpdate;
    
    
    #endregion

    #region Health

    public float myCurrentHealth;
    public bool amIEnemy;
    private EnemyAI myEnemyAI;
    private DamageEffect damageComponent;
    private UnityAction<float> UpdateCharacterHealthListener;
    

    #endregion
    
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
    
    public List<StatModifier> tHPModifiers;
    public List<StatModifier> tSpeedModifiers;
    public List<StatModifier> tAttackModifiers;
    public List<StatModifier> tDefenseModifiers;
    public List<StatModifier> tAttackSpeedModifiers;
    public List<StatModifier> tResistanceModifiers;
    public List<StatModifier> tRRRModifiers; //resource regeneration rate
    public List<StatModifier> tEvasivenessModifiers;
    public List<StatModifier> tTalentModifiers;
    
    public List<StatModifier> pHPModifiers;
    public List<StatModifier> pSpeedModifiers;
    public List<StatModifier> pAttackModifiers;
    public List<StatModifier> pDefenseModifiers;
    public List<StatModifier> pAttackSpeedModifiers;
    public List<StatModifier> pResistanceModifiers;
    public List<StatModifier> pRRRModifiers; //resource regeneration rate
    public List<StatModifier> pEvasivenessModifiers;
    public List<StatModifier> pTalentModifiers;
    
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

    // Method used to apply deserialized stats from prev scene
    public void ApplyStats(PlayerStatsData data)
    {
        baseHP = data.baseHP;
        baseSpeed = data.baseSpeed;
        baseAttack = data.baseAttack;
        baseDefense = data.baseDefense;
        baseAttackSpeed = data.baseAttackSpeed;
        baseResistance = data.baseResistance;
        baseEvasiveness = data.baseEvasiveness;
        baseRRR = data.baseRRR; // Resource regeneration rate
        baseTalent = data.baseTalent;

        ceHP = data.ceHP;
        ceSpeed = data.ceSpeed;
        ceAttack = data.ceAttack;
        ceDefense = data.ceDefense;
        ceAttackSpeed = data.ceAttackSpeed;
        ceResistance = data.ceResistance;
        ceRRR = data.ceRRR; // Resource regeneration rate
        ceEvasiveness = data.ceEvasiveness;
        ceTalent = data.ceTalent;

        // After updating the stats, we might need to call any initialization or update methods
        // that recalculates the derived stats or effects based on these base and current stats.
    }

    private void Start()
    {
        if (!amIEnemy)
        {
            PlayerStatsData data = SaveSystem.LoadPlayerStats();
            if (data != null)
            {
                ApplyStats(data);
            }
        }
        // Moved from start to awake
        tHPModifiers = new List<StatModifier>();
        tSpeedModifiers = new List<StatModifier>();
        tAttackModifiers = new List<StatModifier>();
        tDefenseModifiers = new List<StatModifier>();
        tAttackSpeedModifiers = new List<StatModifier>();
        tResistanceModifiers = new List<StatModifier>();
        tRRRModifiers = new List<StatModifier>();
        tEvasivenessModifiers = new List<StatModifier>();
        tTalentModifiers = new List<StatModifier>();

        pHPModifiers = new List<StatModifier>();
        pSpeedModifiers = new List<StatModifier>();
        pAttackModifiers = new List<StatModifier>();
        pDefenseModifiers = new List<StatModifier>();
        pAttackSpeedModifiers = new List<StatModifier>();
        pResistanceModifiers = new List<StatModifier>();
        pRRRModifiers = new List<StatModifier>();
        pEvasivenessModifiers = new List<StatModifier>();
        pTalentModifiers = new List<StatModifier>();


        ceHP = baseHP;
        ceSpeed = baseSpeed;
        ceAttack = baseAttack;
        ceDefense = baseDefense;
        ceAttackSpeed = baseAttackSpeed;
        ceResistance = baseResistance;
        ceRRR = baseRRR; //resource regeneration rate
        ceEvasiveness = baseEvasiveness;
        ceTalent = baseTalent;

        CalculateCES();

        myEnemyAI = GetComponent<EnemyAI>();
        damageComponent = GetComponentInChildren<DamageEffect>();
        if (myEnemyAI != null)
        {
            amIEnemy = true;
            myCurrentHealth = baseHP;
            OnDisable();
        }
    }

    public void TakeDamage(float damage)
    {
        myCurrentHealth -= damage;
    }

    public void RestoreHealth(float heal)
    {
        myCurrentHealth = Mathf.Min(myCurrentHealth + heal, tHP);
    }

    public void ScaleCurrentHealth(float oldMaxHealth)
    {
        float scalingFactor = tHP / oldMaxHealth;
        myCurrentHealth *= scalingFactor;
        
    }

    private void OnEnable()
    {
        UpdateCharacterHealthListener += TakeDamage;
        EventBus.StartListening(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, UpdateCharacterHealthListener);
    }

    private void OnDisable()
    {
        EventBus.StopListening(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, UpdateCharacterHealthListener);
    }


    

    public void AddModifier(statsList stat, StatModifier modifier)
    {
        switch (stat)
        {
            case statsList.HP:
                tHPModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.HP);
                break;
            case statsList.Speed:
                tSpeedModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Speed);
                break;
            case statsList.Attack:
                tAttackModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Attack);
                break;
            case statsList.Defense:
               tDefenseModifiers.Add(modifier);
               CalculateTemporaryStats(statsList.Defense);
                break;
            case statsList.AttackSpeed:
                tAttackSpeedModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.AttackSpeed);
                break;
            case statsList.Resistance:
                tResistanceModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Resistance);
                break;
            case statsList.RRR:
                tRRRModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Resistance);
                break;
            case statsList.Evasiveness:
                tEvasivenessModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Evasiveness);
                break;
            case statsList.Talent:
                tTalentModifiers.Add(modifier);
                CalculateTemporaryStats(statsList.Talent);
                break;
        }
    }

    public void RemoveModifier(statsList stat, StatModifier modifier)
    {
        switch (stat)
        {
            case statsList.HP:
                tHPModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.HP);
                break;
            case statsList.Speed:
                tSpeedModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Speed);
                break;
            case statsList.Attack:
                tAttackModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Attack);
                break;
            case statsList.Defense:
                tDefenseModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Defense);
                break;
            case statsList.AttackSpeed:
                tAttackSpeedModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.AttackSpeed);
                break;
            case statsList.Resistance:
                tResistanceModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Resistance);
                break;
            case statsList.RRR:
                tRRRModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.RRR);
                break;
            case statsList.Evasiveness:
                tEvasivenessModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Evasiveness);
                break;
            case statsList.Talent:
                tTalentModifiers.Remove(modifier);
                CalculateTemporaryStats(statsList.Talent);
                break;
        }
    }

    
    public void CalculateCES() //calculate and store current effective stat when stats change by sources other than buffs and debuffs
    {
        float delta = 0;
        foreach (StatModifier mod in pHPModifiers)
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
        ceHP += delta;

        delta = 0;
        foreach (StatModifier mod in pAttackModifiers)
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
        ceAttack += delta;
        
        delta = 0;
        foreach (StatModifier mod in pDefenseModifiers)
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
        ceDefense += delta;
        
        delta = 0;
        foreach (StatModifier mod in pSpeedModifiers)
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
        ceSpeed += delta;
        
        delta = 0;
        foreach (StatModifier mod in pAttackSpeedModifiers)
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
        ceAttackSpeed += delta;
        
        delta = 0;
        foreach (StatModifier mod in pResistanceModifiers)
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
        ceResistance += delta;
        delta = 0;
        foreach (StatModifier mod in pRRRModifiers)
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
        ceRRR += delta;
        delta = 0;
        foreach (StatModifier mod in pEvasivenessModifiers)
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
        ceEvasiveness += delta;
        delta = 0;
        foreach (StatModifier mod in pTalentModifiers)
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
        ceTalent += delta;
        CalculateTemporaryStats(statsList.HP);
        CalculateTemporaryStats(statsList.Attack);
        CalculateTemporaryStats(statsList.Defense);
        CalculateTemporaryStats(statsList.Evasiveness);
        CalculateTemporaryStats(statsList.Resistance);
        CalculateTemporaryStats(statsList.Speed);
        CalculateTemporaryStats(statsList.Talent);
        CalculateTemporaryStats(statsList.AttackSpeed);
        CalculateTemporaryStats(statsList.RRR);
    }

    public void
        CalculateTemporaryStats(
            statsList stat) //calculate and store temporary stats whenever buffs/debuffs are applied.
    {
        float delta = 0;
        switch (stat)
        {
            case statsList.HP:

                foreach (StatModifier mod in tHPModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceHP * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                var oldHP = tHP;
                tHP = ceHP + delta;
                ScaleCurrentHealth(oldHP);
                break;
            case statsList.Speed:

                foreach (StatModifier mod in tSpeedModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceSpeed * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }
                
                tSpeed = ceSpeed + delta;
                SpeedUpdate.Invoke(tSpeed);
                break;
            case statsList.Attack:
                foreach (StatModifier mod in tAttackModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceAttack * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tAttack = ceAttack + delta;
                AttackUpdate.Invoke(tAttack);
                break;
            case statsList.Defense:

                foreach (StatModifier mod in tDefenseModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceDefense * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tDefense = ceDefense + delta;
                break;
            case statsList.AttackSpeed:
                foreach (StatModifier mod in tAttackSpeedModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceAttackSpeed * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tAttackSpeed = ceAttackSpeed + delta;
                AttackSpeedUpdate.Invoke(tAttackSpeed);
                break;
            case statsList.Resistance:
                foreach (StatModifier mod in tResistanceModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceResistance * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tResistance = ceResistance + delta;
                break;
            case statsList.RRR:
                foreach (StatModifier mod in tRRRModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceRRR * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tRRR = ceRRR + delta;
                break;
            case statsList.Evasiveness:
                foreach (StatModifier mod in tEvasivenessModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceEvasiveness * mod.value / 100;
                    }
                    else
                    {
                        delta += mod.value;
                    }
                }

                tEvasiveness = ceEvasiveness + delta;
                break;
            case statsList.Talent:
                foreach (StatModifier mod in tTalentModifiers)
                {
                    if (mod.myType == StatModifierType.Percent)
                    {
                        delta += ceTalent * mod.value / 100;
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
