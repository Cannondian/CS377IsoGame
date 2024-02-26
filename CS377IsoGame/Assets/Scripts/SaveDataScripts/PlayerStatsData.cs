using System;

[Serializable]
public class PlayerStatsData
{
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

    // Empty constructor for deserialization 
    public PlayerStatsData() { }

    // Constructor to initialize from a StatsTemplate instance
    public PlayerStatsData(StatsTemplate stats)
    {
        baseHP = stats.baseHP;
        baseSpeed = stats.baseSpeed;
        baseAttack = stats.baseAttack;
        baseDefense = stats.baseDefense;
        baseAttackSpeed = stats.baseAttackSpeed;
        baseResistance = stats.baseResistance;
        baseEvasiveness = stats.baseEvasiveness;
        baseRRR = stats.baseRRR;
        baseTalent = stats.baseTalent;

        ceHP = stats.ceHP;
        ceSpeed = stats.ceSpeed;
        ceAttack = stats.ceAttack;
        ceDefense = stats.ceDefense;
        ceAttackSpeed = stats.ceAttackSpeed;
        ceResistance = stats.ceResistance;
        ceRRR = stats.ceRRR;
        ceEvasiveness = stats.ceEvasiveness;
        ceTalent = stats.ceTalent;
    }
}
