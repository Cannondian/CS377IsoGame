using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using RPGCharacterAnims.Actions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = System.Random;

public class ConditionState : MonoBehaviour
{

    public UnityEvent<StatusConditions.statusList, bool> communicateWithUI;
    
    
    
    #region Components

    private StatsTemplate myStats;
    private FXHandler FXhandler;
    public bool enemy;

    #endregion
    
    #region DebuffStatusFlags

    public bool burning;
    [FormerlySerializedAs("corrosive")] public bool poisoned;
    public bool bleeding;
    public bool confused;
    public bool stunned;
    public bool slowed;
    public bool exhausted;
    public bool blinded;
    public bool hacked;
    public bool radiationPoisoning;
   

    #endregion
    

    #region BuffStatusFlags

    public bool rejuvenation;
    public bool energized;
    public bool slipperySteps;
    public bool smolderingStrikes;
    public bool evasive;
    public bool oneWithTheWorld;
    public bool unstoppable;
    public bool mutating;
    public bool glowing;
    public bool defensiveTerrain;
    public bool chlorophyllInfusion;

    #endregion
    
    #region JisaStatusFlags
    
    
    
    #endregion
    
    #region BurningData

    public float burningDuration;
    public float burningCounter;
    

    #endregion

    #region PoisonedData

    private float poisonedDuration;
    private float poisonedIntensity;
    private StatModifier poisonedModifier;
    private int poisonedStackCount;
    private float poisonedCounter;
    #endregion
    
    #region BleedingData

    private float bleedingDuration;
    private int bleedingStackCount;
   
    
    #endregion
    
    #region ConfusedData

    private float confusedDuration;
    
    
    #endregion

    #region StunnedData

    private float stunnedDuration;
    

    #endregion

    #region CholorphyllInfusionData

    private float chlorophyllInfusionDuration;
    private float chlorophyllInfusionIntensity;
    private float chlorophyllInfusionCounter;

    #endregion
    
    
    
    #region SlowedData

    public StatModifier slowModifier; //stat modifier that represents all active slow effect on the entity
    public struct Slow
    {
        public readonly float slowIntensity;
        public readonly float slowDuration;
        
        public Slow(float i, float d)
        {
            slowIntensity = i;
            slowDuration = d;
        }
    }

    private List<Slow> activeSlows;
    private float effectiveSlowIntensity;
    
    #endregion

    #region InnderFireData

    public bool innerFire;
    private float innerFireDuration;
    public float innerFireIntensity;
    
    #endregion
    #region ExhaustedData

    private float exhaustedDuration;
    private StatModifier exhaustedModifier;

    #endregion

    #region  RejuvenationData

    public float effectiveRejuvenationIntensityforFX;
    public float rejuvinationCounter;
    public float rejuvenationDuration;
    public struct Rejuvenation
    {
        public float duration;
        public float intensity;

        public Rejuvenation(float d, float i)
        {
            duration = d;
            intensity = i;
        }

    }

    private List<Rejuvenation> activeRejuvenations;

    #endregion

    #region EnergizedData

    private float energizedDuration;
    private StatModifier energizedModifier;

    #endregion

    #region SlipperyStepsData

    private float slipperyStepsDuration;
    private float slipperyStepsIntensity;
    private StatModifier slipperyStepsSpeedModifier;
    private StatModifier slipperyStepsResistanceModifier;
    private StatModifier slipperyStepsAttackSpeedModifier;

    #endregion

    #region SmolderingStrikesData

    private float smolderingStrikesCounter;
    private float smolderingStrikesIntensity;
    private float smolderingStrikesDuration;
    private StatModifier smolderingStrikesModifier;
    private PlayerHitEffects.HitEffect smolderingStrikesHitEffect;

    #endregion
    
    #region EvasiveData

    private float evasiveDuration;
    private StatModifier evasiveModifier;
    
    #endregion
    
    #region DefensiveTerrainData

    private float defensiveTerrainDuration;
    private StatModifier defensiveTerrainModifier;
    private float defensiveTerrainIntensity;
    
    #endregion

    #region MutatingData

    private float mutatingDuration;
    

    #endregion

    #region RadiationPoisoningData

    private float radiationPoisoningDuration;

    #endregion

    #region GlowingData

    private float glowingDuration;
    private StatModifier glowingModifier;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        activeSlows = new List<Slow>();
        activeRejuvenations = new List<Rejuvenation>();
        myStats = GetComponent<StatsTemplate>();
        FXhandler = FXHandler.Instance;
        enemy = myStats.amIEnemy;

        //make modifiers for each of the status conditions.
        //These modifiers will be sent to the stat template for adjusting of those stats
        //once the status condition is applied
        //status conditions that have pure HP increasing/decreasing effects are not represented as 
        //modifiers in the stat template, unless they alter the max HP of the entity;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (burning)
        {
            ApplyBurning();
        }

        if (poisoned)
        {
            ApplyPoisoned();
        }

        if (bleeding)
        {
            ApplyBleeding();
        }
    
        if (confused)
        {
            ApplyConfused();
        }
    
        if (stunned)
        {
            ApplyStunned();
        }
    
        if (slowed)
        {
            ApplySlowed();
        }

        if (exhausted)
        {
            ApplyExhausted();
        }
    
        if (blinded)
        {
            ApplyBlinded();
        }

        if (hacked)
        {
            ApplyHacked();
        }
    
        if (radiationPoisoning)
        {
            ApplyRadiationPoisoning();
        }

// -- Buff status flags

        if (rejuvenation)
        {
            ApplyRejuvenation();
        }

        if (energized)
        {
            ApplyEnergized();
        }

        if (slipperySteps)
        {
            ApplySlipperySteps();
        }

        if (smolderingStrikes)
        {
            ApplySmolderingStrikes();
        }

        if (evasive)
        {
            ApplyEvasive();
        }

        if (oneWithTheWorld)
        {
            ApplyOneWithTheWorld();
        }

        if (unstoppable)
        {
            ApplyUnstoppable();
        }

        if (mutating)
        {
            ApplyMutating();
        }

        if (glowing)
        {
            ApplyGlowing();
        }

        if (defensiveTerrain)
        {
            ApplyDefensiveTerrain();
        }

        if (chlorophyllInfusion)
        {
            ApplyChlorophyllInfusion();
        }
        
    }
/// <summary>
/// Sets, or updates a condition if reapplied. Takes in any and all inputs required for a given status condition.
/// Most parameters are optional.
/// Setting a condition consists of a few steps. We update the duration of conditions that are already present.
/// We set the flags and initialize FX for conditions that were not present previously.
/// If a condition is applied with a greater intensity than the previous application on the entity,
/// we also update FX in that case to trigger larger FX.
/// Then, we create modifiers. If the status was already present before the most recent application,
/// and it's specifics were not updates, then we keep the current modifier for that status and do not make a new one.
/// If the status is newly applied, we make modifiers for it, send it to the stat template for the changed to take place,
/// and save that value so that we can send a message to the stat template fr it to be removed when appropriate.
/// </summary>
/// <param name="condition"></param>
/// <param name="duration"></param>
/// <param name="intensity"></param>
    public void SetCondition(StatusConditions.statusList condition, float duration = -1, float intensity = 1)
    {
        communicateWithUI.Invoke(condition, true);
        switch (condition)
        {
            case StatusConditions.statusList.Burning:
                if (burning)
                {
                    if (duration > burningDuration) burningDuration = duration; //refresh duration
                }
                else
                {
                    burning = true;
                    if (enemy)
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXHandler.Instance.BurningFXPrefabEnemy, gameObject,
                                nameof(StatusConditions.statusList.Burning)));
                    }
                    else
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXHandler.Instance.playerBurningFX, gameObject,
                                nameof(StatusConditions.statusList.Burning)));
                    }
                    
                    burningDuration = duration;
                }
                //ui trigger will go here
                
                break;
            case StatusConditions.statusList.Poisoned: 

                if (poisonedStackCount < 10)
                {
                    if (poisoned) 
                    { 
                        myStats.RemoveModifier(StatsTemplate.statsList.HP, poisonedModifier);
                       

                    }
                    poisonedStackCount += 1;
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.PoisonedFXPrefab, gameObject,
                                                                nameof(StatusConditions.statusList.Poisoned), intensity));
                    poisonedModifier = new StatModifier(intensity * -3 * poisonedStackCount, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.HP, poisonedModifier);
                }

                poisoned = true;
                if (duration > poisonedDuration)
                {
                    poisonedDuration = duration;
                }

                break;
            //intensity can bu used to apply multiple stacks at once
            case StatusConditions.statusList.Bleeding:
                if (!bleeding)
                {
                    bleeding = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.BleedingFXPrefab,
                            gameObject, nameof(StatusConditions.statusList.Bleeding)));
                }

                if (bleedingStackCount < 10) bleedingStackCount += 1;
                
                if (duration > bleedingDuration)
                {
                    bleedingDuration = duration;
                }
                break;
            case StatusConditions.statusList.Confused:
                
                    if (!confused)
                    {
                        confused = true;
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.ConfusedFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.Confused)));
                    }
                    if (duration > confusedDuration)
                    {
                        confusedDuration = duration;
                    }
                

                break;
            case StatusConditions.statusList.InnerFire:
                if (!innerFire && enemy)
                {
                    innerFire = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.InnerFireFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.InnerFire)));
                    innerFireDuration = duration;
                    innerFireIntensity = intensity;
                }

                break;
            case StatusConditions.statusList.Stunned:
                
                    if (!stunned)
                    {
                        stunned = true;
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.StunnedFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.Stunned)));
                    }
                    if (duration > stunnedDuration) stunnedDuration = duration;
                

                break;
            case StatusConditions.statusList.Slow: //intensity level between 1-18, corresponding to scaled multiples of 5%. 
                
                    if (!slowed)
                    {
                        slowed = true;
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.SlowFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.Slow)));
                        // add a check here?
                        activeSlows[0] = new Slow(intensity, duration);
                        slowModifier = new StatModifier(intensity * -5, StatModifierType.Percent);
                        myStats.AddModifier(StatsTemplate.statsList.Speed, slowModifier);
                    }
                    //this is an additive debuff, which means consecutive different intensity applications stack, 
                    else if (activeSlows.Count <=
                        10) //however, only the current active debuff with the greatest intensity actually affects the player
                    {
                        activeSlows.Add(new Slow(intensity, duration));
                        
                        if (effectiveSlowIntensity < intensity)
                        {
                            effectiveSlowIntensity = intensity;
                            myStats.RemoveModifier(StatsTemplate.statsList.Speed, slowModifier);
                            slowModifier = new StatModifier(intensity * -5, StatModifierType.Percent);
                            myStats.AddModifier(StatsTemplate.statsList.Speed, slowModifier);
                        }
                    }
                

                break;
            case StatusConditions.statusList.Exhausted:
                if (!exhausted)
                {
                    exhausted = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.ExhaustedFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Exhausted)));
                    exhaustedModifier = new StatModifier(-30, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.RRR, exhaustedModifier);
                }
                if(exhaustedDuration < duration) exhaustedDuration = duration;
                break;
            case StatusConditions.statusList.Rejuvenation: //intensity level between 1-4, intensity determines FX, yet multiple can be active at once, 
                if (effectiveRejuvenationIntensityforFX < intensity)
                {
                    if (rejuvenation)
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.RejuvenationFXPrefab,
                                gameObject, nameof(StatusConditions.statusList.Rejuvenation), intensity));
                    }

                    rejuvenation = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.RejuvenationFXPrefab,
                            gameObject, nameof(StatusConditions.statusList.Rejuvenation), intensity));
                    effectiveRejuvenationIntensityforFX = intensity;
                    rejuvenationDuration = duration;
                    if (activeRejuvenations.Count == 4)
                    {
                        Rejuvenation rejuvenationToRemove = activeRejuvenations[0];
                        float lowest = activeRejuvenations[0].intensity;

                        foreach (Rejuvenation r in activeRejuvenations)
                        {
                            if (r.intensity < lowest)
                            {
                                lowest = r.intensity;
                                rejuvenationToRemove = r;
                            }

                        }

                        activeRejuvenations.Remove(rejuvenationToRemove);
                        activeRejuvenations.Add(new Rejuvenation(duration, intensity));
                    }
                    else
                    {
                        activeRejuvenations.Add(new Rejuvenation(duration, intensity));
                    }
                }

                break;
            case StatusConditions.statusList.ChlorophyllInfusion:
                if (!chlorophyllInfusion && !enemy)
                {
                   
                    if (smolderingStrikes)
                    {
                       RemoveCondition(StatusConditions.statusList.SmolderingStrikes);
                    }

                    if (slipperySteps)
                    {
                        RemoveCondition(StatusConditions.statusList.SlipperySteps);
                    }
                    
                    chlorophyllInfusion = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_HIGHLIGHT_FX_TRIGGERED, 
                        new EventTypes.HighlightFXParam(FXhandler.Velheret, intensity, false));
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION, 
                        new EventTypes.StatusConditionFXParam( FXhandler.ChlorophyllInfusionPrefab, this.gameObject, 
                            nameof(StatusConditions.statusList.ChlorophyllInfusion)));
                }

                chlorophyllInfusionDuration = duration;
                break;
            case StatusConditions.statusList.Energized:
                if (!energized)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION, 
                        new EventTypes.StatusConditionFXParam(FXhandler.EnergizedFXPrefab, this.gameObject,
                            nameof(StatusConditions.statusList.Energized)));
                    energizedModifier = new StatModifier(30, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.RRR, energizedModifier);
                    energized = true;
                }

                if (duration > energizedDuration) energizedDuration = duration;
                break;
            //buff with the highest intensity is used. Duration is constant for every application and is always reset
            //intensity between 1-3;
            //at intensity 3, also increases attack speed
            //stacks basically
            case StatusConditions.statusList.SlipperySteps:
                if (intensity > slipperyStepsIntensity) 
                {
                    
                    if (smolderingStrikes)
                    {
                        RemoveCondition(StatusConditions.statusList.SmolderingStrikes);
                    }

                    if (chlorophyllInfusion)
                    {
                        RemoveCondition(StatusConditions.statusList.ChlorophyllInfusion);
                    }
                    
                    
                    if (slipperySteps)
                    {
                        myStats.RemoveModifier(StatsTemplate.statsList.Speed, slipperyStepsSpeedModifier);
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.SlipperyStepsFXPrefab, this.gameObject,
                                nameof(StatusConditions.statusList.SlipperySteps)));
                    }

                    slipperyStepsSpeedModifier =
                        new StatModifier(intensity * 15, StatModifierType.Percent); //add new version
                    myStats.AddModifier(StatsTemplate.statsList.Speed, slipperyStepsSpeedModifier);
                    slipperyStepsResistanceModifier = new StatModifier(100 * intensity, StatModifierType.Flat);
                    myStats.AddModifier(StatsTemplate.statsList.Resistance, slipperyStepsResistanceModifier);
                    Debug.Log("slippery");
                    if (!enemy)
                    {
                        slipperyStepsAttackSpeedModifier = new StatModifier(0.2f * intensity, StatModifierType.Flat);
                        myStats.AddModifier(StatsTemplate.statsList.AttackSpeed, slipperyStepsAttackSpeedModifier);
                    }

                    EventBus.TriggerEvent(
                        EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(
                            FXhandler.SlipperyStepsFXPrefab,
                            this.gameObject,
                            nameof(StatusConditions.statusList
                                .SlipperySteps)));
                    if (!enemy)
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_HIGHLIGHT_FX_TRIGGERED, 
                            new EventTypes.HighlightFXParam(FXhandler.Shalharan, intensity, false));
                    }
                }

                slipperyStepsIntensity = intensity;
                slipperySteps = true;
                slipperyStepsDuration = duration;
                
                break;
            //scales with attack but has fixed intensity
            //also increases attack minimally
            case StatusConditions.statusList.SmolderingStrikes:
                if (!enemy)
                {
                    SetCondition(StatusConditions.statusList.Burning, duration, intensity);
                    if (!smolderingStrikes)
                    {
                        if (slipperySteps)
                        {
                            RemoveCondition(StatusConditions.statusList.SlipperySteps);
                        }

                        if (chlorophyllInfusion)
                        {
                            RemoveCondition(StatusConditions.statusList.ChlorophyllInfusion);
                        }

                        
                        EventBus.TriggerEvent(EventTypes.Events.ON_HIGHLIGHT_FX_TRIGGERED,
                            new EventTypes.HighlightFXParam(FXhandler.Ilsihre, 1, false));
                               
                        Debug.Log("smoldering strikes2");

                        smolderingStrikesModifier = new StatModifier(10 * intensity, StatModifierType.Percent);
                        myStats.AddModifier(StatsTemplate.statsList.Attack, smolderingStrikesModifier);
                        
                            PlayerHitEffects.HitEffect.AdditionalEffect smolderingStrikesHitFunction = (self, other) =>
                            {
                                other.GetComponent<ConditionState>()
                                    .SetCondition(StatusConditions.statusList.Burning, 4 * intensity);
                            };
                            smolderingStrikesHitEffect = new PlayerHitEffects.HitEffect(
                                myStats.tAttack * 0.2f * Mathf.Pow(intensity, 2),
                                smolderingStrikesHitFunction);
                            PlayerHitEffects.Instance.AddHitEffect(smolderingStrikesHitEffect);
                        
                    }

                    smolderingStrikes = true;
                    
                    if (smolderingStrikesDuration < duration)
                    {
                        smolderingStrikesDuration = duration;
                    }
                }

                break;
            case StatusConditions.statusList.Evasive:
                if (!evasive)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION, 
                        new EventTypes.StatusConditionFXParam(FXhandler.EvasiveFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Evasive)));
                    evasiveModifier = new StatModifier(30, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.Evasiveness, evasiveModifier);
                }

                evasive = true;
                if (evasiveDuration < duration) evasiveDuration = duration;
                break;
            case StatusConditions.statusList.Blinded:
                // handle Blinded condition
                break;
            case StatusConditions.statusList.OneWithTheWorld:
                // handle OneWithTheWorld condition
                break;
            case StatusConditions.statusList.Hacked:
                // handle Hacked condition
                break;
            case StatusConditions.statusList.Unstoppable:
                // handle Unstoppable condition
                break;
            case StatusConditions.statusList.RadiationPoisoning:
                if (!radiationPoisoning)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.RadiationPoisoningFXPrefab, this.gameObject,
                            nameof(StatusConditions.statusList.RadiationPoisoning)));
                    radiationPoisoning = true;
                    radiationPoisoningDuration = duration;
                    SetRandomDebuffs(duration);
                    
                }
                
                break;
            
            case StatusConditions.statusList.Mutating:
                if (!mutating)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.MutatingFXPrefab, this.gameObject,
                            nameof(StatusConditions.statusList.Mutating)));
                    mutating = true;
                    mutatingDuration = duration;
                    SetRandomBuffs(duration);
                    
                }

                break;
            case StatusConditions.statusList.Glowing:
                if (!glowing)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.GlowingFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Glowing)));
                    glowingModifier = new StatModifier(15, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.AttackSpeed,glowingModifier);
                }
                /* reminder to add HitEffect here*/
                glowing = true;
                if (glowingDuration < duration) glowingDuration = duration;
                break;
            case StatusConditions.statusList.DefensiveTerrain: //intensity is determined by the amount of rocks within a given terrain tile, scales with defense.
                if (defensiveTerrainIntensity < intensity)
                {
                    if (defensiveTerrain) 
                    { 
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION, 
                            new EventTypes.StatusConditionFXParam(FXhandler.DefensiveTerrainFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.DefensiveTerrain)));
                        myStats.RemoveModifier(StatsTemplate.statsList.Defense, defensiveTerrainModifier);
                    }
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.DefensiveTerrainFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.DefensiveTerrain), intensity));
                    
                    defensiveTerrainModifier = new StatModifier(intensity * 20, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.Defense, defensiveTerrainModifier);
                }

                defensiveTerrain = true;
                if (duration > defensiveTerrainDuration)
                {
                    defensiveTerrainDuration = duration;
                }
                break;
        }
    }
    //set two random buffs and make sure they are not the same
    private void SetRandomBuffs(float duration)
    {
        Random random = new Random();
        int intensity = 1;
        var values = (IList)Enum.GetValues(typeof(StatusConditions.Buffs));
        StatusConditions.Buffs buff1 = (StatusConditions.Buffs)values[random.Next(0, values.Count - 1)];
        intensity = GenerateRandomBuffIntensity(buff1);
        SetCondition((StatusConditions.statusList)(int)buff1, duration, intensity);



        StatusConditions.Buffs buff2 = (StatusConditions.Buffs)values[random.Next(0, values.Count - 1)];
        while (buff1 == buff2)
        {
            buff2 = (StatusConditions.Buffs)values[random.Next(0, values.Count - 1)];

        }

        intensity = GenerateRandomBuffIntensity(buff2);
        SetCondition((StatusConditions.statusList)(int)buff2, duration, intensity);
    }

    private int GenerateRandomBuffIntensity(StatusConditions.Buffs buff)
    {
        int i = 1;
        Random random = new Random();
        if (buff.Equals(StatusConditions.Buffs.Rejuvenation))
        {
            i = random.Next(1, 4);
        }
        else if (buff.Equals(StatusConditions.Buffs.SlipperySteps))
        {
            i = random.Next(1, 3);
        }
        else if (buff.Equals(StatusConditions.Buffs.DefensiveTerrain))
        {
            i = random.Next(1, 4);
        }
        
        return i;
        
    }
    
    //set two random debuffs and make sure they are not the same
    private void SetRandomDebuffs(float duration)
    {
        Random random = new Random();
        int intensity = 1;
        var values = (IList)Enum.GetValues(typeof(StatusConditions.Debuffs));
        StatusConditions.Debuffs debuff1 = (StatusConditions.Debuffs)values[random.Next(0, values.Count - 1)];
        intensity = GenerateRandomDebuffIntensity(debuff1);
        SetCondition((StatusConditions.statusList)(int)debuff1, duration, intensity);



        StatusConditions.Debuffs debuff2 = (StatusConditions.Debuffs)values[random.Next(0, values.Count - 1)];
        while (debuff1 == debuff2)
        {
            debuff2 = (StatusConditions.Debuffs)values[random.Next(0, values.Count - 1)];

        }

        intensity = GenerateRandomDebuffIntensity(debuff2);
        SetCondition((StatusConditions.statusList)(int)debuff2, duration, intensity);
    }
    private int GenerateRandomDebuffIntensity(StatusConditions.Debuffs debuff)
    {
        int i = 1;
        Random random = new Random();
        if (debuff.Equals(StatusConditions.Debuffs.Bleeding))
        {
            i = random.Next(1, 10);
        }
        else if (debuff.Equals(StatusConditions.Debuffs.Slow))
        {
            i = random.Next(1, 18);
        }
        else if (debuff.Equals(StatusConditions.Debuffs.Corrosive))
        {
            i = random.Next(1, 3);
        }
        
        return i;
    }
    
    
    public void GetCondition(StatusConditions.statusList condition)
    {
        
    }

    //remove condition with single application, or if multiple applications exist, remove the condition instance atIndex
    public void RemoveCondition(StatusConditions.statusList condition, int atIndex = 0)
    {
        communicateWithUI.Invoke(condition, false);
        switch (condition)
        {
            case StatusConditions.statusList.Burning:

                if (burning)
                {
                    burningDuration = 0;
                    burning = false;
                    if (enemy)
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXHandler.Instance.BurningFXPrefabEnemy, gameObject,
                                nameof(StatusConditions.statusList.Burning)));
                    }
                    else
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXHandler.Instance.playerBurningFX, gameObject,
                                nameof(StatusConditions.statusList.Burning)));
                    }
                }

                break;

            case StatusConditions.statusList.Poisoned: //intensity levels are 1, 2 and 3


                if (poisoned)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.PoisonedFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Poisoned)));
                    myStats.RemoveModifier(StatsTemplate.statsList.HP, poisonedModifier);
                    myStats.UnscaleCurrentHealth();
                    poisoned = false;
                    poisonedDuration = 0;
                    poisonedIntensity = 0;
                    poisonedStackCount = 0;
                }

                break;
            //intensity can bu used to apply multiple stacks at once
            case StatusConditions.statusList.InnerFire:
                if (innerFire)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.InnerFireFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.InnerFire)));
                    innerFire = false;
                    innerFireDuration = 0;
                    innerFireIntensity = 0;
                }

                break;
            
            
            case StatusConditions.statusList.Bleeding:
                if (bleeding)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.BleedingFXPrefab,
                            gameObject, nameof(StatusConditions.statusList.Bleeding)));
                    bleeding = false;
                    bleedingDuration = 0;
                    bleedingStackCount = 0;
                }

                break;
            case StatusConditions.statusList.Confused:

                if (confused)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.ConfusedFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Confused)));
                    confused = false;
                    confusedDuration = 0;

                }

                break;
            case StatusConditions.statusList.Stunned:

                if (stunned)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.StunnedFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Stunned)));
                    stunned = false;
                    stunnedDuration = 0;
                }



                break;
            case StatusConditions.statusList.Slow
                : //intensity level between 1-18, corresponding to scaled multiples of 5%. 

                if (slowed)
                {
                    if (activeSlows.Count != 1)
                    {
                        if (activeSlows[atIndex].slowIntensity == effectiveSlowIntensity)
                        {
                            float nextLargestSlow = 0;
                            activeSlows.RemoveAt(atIndex);
                            foreach (Slow s in activeSlows)
                            {
                                if (s.slowIntensity > nextLargestSlow)
                                {
                                    nextLargestSlow = s.slowIntensity;
                                }
                            }


                            effectiveSlowIntensity = nextLargestSlow;
                            myStats.RemoveModifier(StatsTemplate.statsList.Speed, slowModifier);
                            slowModifier = new StatModifier(-5 * effectiveSlowIntensity,
                                StatModifierType.Percent);
                            myStats.AddModifier(StatsTemplate.statsList.Speed, slowModifier);


                        }
                        else
                        {
                            activeSlows.RemoveAt(atIndex);
                        }
                    }
                    else
                    {
                        activeSlows.RemoveAt(atIndex);
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.SlowFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.Slow)));
                        effectiveSlowIntensity = 0;
                    }

                }

                break;
            case StatusConditions.statusList.Exhausted:
                if (exhausted)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.ExhaustedFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Exhausted)));
                    myStats.RemoveModifier(StatsTemplate.statsList.RRR, exhaustedModifier);
                    exhausted = false;
                    exhaustedDuration = 0;
                }

                break;
            case StatusConditions.statusList.Rejuvenation
                : //intensity level between 1-4, intensity determines FX, yet multiple can be active at once, 

                Rejuvenation rejuvenationToRemove = activeRejuvenations[atIndex];
                if (rejuvenation)
                {
                    activeRejuvenations.Remove(rejuvenationToRemove);
                    if (activeRejuvenations.Count == 0)
                    {
                        rejuvenation = false;
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.RejuvenationFXPrefab,
                                gameObject, nameof(StatusConditions.statusList.Rejuvenation),
                                rejuvenationToRemove.intensity));
                        effectiveRejuvenationIntensityforFX = 0;
                    }

                    else if (rejuvenationToRemove.intensity == effectiveRejuvenationIntensityforFX)
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.RejuvenationFXPrefab,
                                gameObject, nameof(StatusConditions.statusList.Rejuvenation),
                                rejuvenationToRemove.intensity));
                        activeRejuvenations.Remove(rejuvenationToRemove);
                        float largestRejuvenation = 0;
                        foreach (Rejuvenation r in activeRejuvenations)
                        {
                            if (r.intensity > largestRejuvenation)
                            {
                                largestRejuvenation = r.intensity;
                            }
                        }

                        effectiveRejuvenationIntensityforFX = largestRejuvenation;
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.RejuvenationFXPrefab,
                                gameObject, nameof(StatusConditions.statusList.Rejuvenation), largestRejuvenation));

                    }
                }

                break;
            case StatusConditions.statusList.Energized:
                if (energized)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.EnergizedFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Energized)));
                    myStats.RemoveModifier(StatsTemplate.statsList.RRR, energizedModifier);
                    energized = false;
                    energizedDuration = 0;
                }

                break;
            //buff with the highest intensity is used. Duration is constant for every application and is always reset
            //intensity between 1-3;
            //at intensity 3, also increases attack speed
            //stacks basically
            case StatusConditions.statusList.SlipperySteps:
                if (slipperySteps)
                {
                   
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.SlipperyStepsFXPrefab, this.gameObject,
                            nameof(StatusConditions.statusList.SlipperySteps)));
                    
                    EventBus.TriggerEvent(EventTypes.Events.ON_HIGHLIGHT_FX_EXPIRED,
                        new EventTypes.HighlightFXParam(FXhandler.Shalharan, 0, false));
                    
                    if (!enemy)
                    {
                        myStats.RemoveModifier(StatsTemplate.statsList.AttackSpeed, slipperyStepsAttackSpeedModifier);
                    }
                    myStats.RemoveModifier(StatsTemplate.statsList.Resistance, slipperyStepsResistanceModifier);
                   
                    myStats.RemoveModifier(StatsTemplate.statsList.Speed, slipperyStepsSpeedModifier);
                    
                    slipperyStepsDuration = 0;
                    slipperyStepsIntensity = 0;
                    slipperySteps = false;
                }

                break;
            //scales with attack but has fixed intensity
            //also increases attack minimally
            case StatusConditions.statusList.SmolderingStrikes:
                if (smolderingStrikes && !enemy)
                {
                  //if we want to replace it with another, then pass on a different highlight than the one that's already in effect;
                    EventBus.TriggerEvent(EventTypes.Events.ON_HIGHLIGHT_FX_EXPIRED, 
                      new EventTypes.HighlightFXParam(FXhandler.Ilsihre, 0, false));

                  myStats.RemoveModifier(StatsTemplate.statsList.Attack, smolderingStrikesModifier);
                  PlayerHitEffects.Instance.RemoveHitEffect(smolderingStrikesHitEffect);
                  smolderingStrikes = false;
                  smolderingStrikesDuration = 0;
                }
                
                break;
            case StatusConditions.statusList.ChlorophyllInfusion:
                if (chlorophyllInfusion)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.ChlorophyllInfusionPrefab, gameObject,
                            nameof(StatusConditions.statusList.ChlorophyllInfusion)));
                    EventBus.TriggerEvent(EventTypes.Events.ON_HIGHLIGHT_FX_EXPIRED,
                        new EventTypes.HighlightFXParam(FXhandler.Velheret, 1, false));
                    chlorophyllInfusionDuration = 0;
                    chlorophyllInfusionIntensity = 0;
                    chlorophyllInfusion = false;
                    myStats.bonusHealth = 0;
                    
                }

                break;
            case StatusConditions.statusList.Evasive:
                if (evasive)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.EvasiveFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Evasive)));
                    myStats.RemoveModifier(StatsTemplate.statsList.Evasiveness, evasiveModifier);
                    evasive = false;
                    evasiveDuration = 0;
                }
                break;
            case StatusConditions.statusList.Blinded:
                // handle Blinded condition
                break;
            case StatusConditions.statusList.OneWithTheWorld:
                // handle OneWithTheWorld condition
                break;
            case StatusConditions.statusList.Hacked:
                // handle Hacked condition
                break;
            case StatusConditions.statusList.Unstoppable:
                // handle Unstoppable condition
                break;
            case StatusConditions.statusList.RadiationPoisoning:
                if (radiationPoisoning)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.RadiationPoisoningFXPrefab, this.gameObject,
                            nameof(StatusConditions.statusList.RadiationPoisoning)));
                    radiationPoisoning = false;
                    radiationPoisoningDuration = 0;


                }

                break;

            case StatusConditions.statusList.Mutating:
                if (mutating)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.MutatingFXPrefab, this.gameObject,
                            nameof(StatusConditions.statusList.Mutating)));
                    mutating = false;
                    mutatingDuration = 0;

                }

                break;
            case StatusConditions.statusList.Glowing:
                if (glowing)
                {
                    EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.GlowingFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Glowing)));
                    myStats.RemoveModifier(StatsTemplate.statsList.AttackSpeed,glowingModifier);
                    glowing = false;
                    glowingDuration = 0;
                }
                /* reminder to add HitEffect here*/ 
                
                break;
            case StatusConditions.statusList.DefensiveTerrain: //intensity is determined by the amount of rocks within a given terrain tile, scales with defense.
                if (defensiveTerrain)
                {
                    
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.DefensiveTerrainFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.DefensiveTerrain)));
                        myStats.RemoveModifier(StatsTemplate.statsList.Defense, defensiveTerrainModifier);
                        defensiveTerrain = false;
                        defensiveTerrainDuration = 0;
                        defensiveTerrainIntensity = 0;
                }
                break;
        }
    }


    private void AdjustDefensiveTerrain()
    {
    }
    

    private void ApplyBurning()
    {
        burningCounter += Time.deltaTime;
        
        if (burningDuration <= 0)
        {
            RemoveCondition(StatusConditions.statusList.Burning);
            burningCounter = 0;
        }

        if (burningCounter > 1 )
        {
            
            burningDuration -= burningCounter;
            burningCounter = 0;
            var tickDamage = myStats.tHP * 0.02f;
            if (myStats.myCurrentHealth > tickDamage)
            {
                myStats.TakeDamage(tickDamage);
                EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT,
                    new EventTypes.FloatingDamageParam(gameObject, tickDamage, 1,
                        Damage.Types.Fire));
            }
        }
       
    }

    private void ApplyPoisoned()
    {
        poisonedCounter += Time.deltaTime;
        
        if (poisonedDuration <= 0)
        {
            RemoveCondition(StatusConditions.statusList.Poisoned);
            poisonedCounter = 0;
        }

        if (poisonedCounter > 1)
        {

            poisonedDuration -= poisonedCounter;
            poisonedCounter = 0;
        }
    }

    private void ApplyBleeding()
    {
        // Insert logic for 'Bleeding' state here
    }

    private void ApplyConfused()
    {
        // Insert logic for 'Confused' state here
    }

    private void ApplyStunned()
    {
        // Insert logic for 'Stunned' state here
    }

    private void ApplySlowed()
    {
        // Insert logic for 'Slowed' state here
    }

    private void ApplyExhausted()
    {
        // Insert logic for 'Exhausted' state here
    }

    private void ApplyBlinded()
    {
        // Insert logic for 'Blinded' state here
    }

    private void ApplyHacked()
    {
        // Insert logic for 'Hacked' state here
    }

    private void ApplyRadiationPoisoning()
    {
        // Insert logic for 'RadiationPoisoning' state here
    }

    private void ApplyRejuvenation()
    {
        rejuvinationCounter += Time.deltaTime;
        var rej = activeRejuvenations[0];
        if (rejuvenationDuration <= 0)
        {
            RemoveCondition(StatusConditions.statusList.Rejuvenation, 0);
            rejuvinationCounter = 0;
        }

        if (rejuvinationCounter > 0.5f)
        {
            rejuvenationDuration -= rejuvinationCounter;
            rejuvinationCounter = 0;
            int heal = (int)rej.intensity / 20 + 2;
            myStats.RestoreHealth(heal);
        }
    }

    private void ApplyEnergized()
    {
        // Insert logic for 'Energized' state here
    }

    private void ApplySlipperySteps()
    {
        // Insert logic for 'SlipperySteps' state here
    }

    private void ApplySmolderingStrikes()
    {
        smolderingStrikesDuration -= Time.deltaTime;
        if (smolderingStrikesDuration <= 0)
        {
            RemoveCondition(StatusConditions.statusList.SmolderingStrikes);
            RemoveCondition(StatusConditions.statusList.Burning);
        }
    }

    private void ApplyEvasive()
    {
        // Insert logic for 'Evasive' state here
    }

    private void ApplyOneWithTheWorld()
    {
        // Insert logic for 'OneWithTheWorld' state here
    }

    private void ApplyUnstoppable()
    {
        // Insert logic for 'Unstoppable' state here
    }

    private void ApplyMutating()
    {
        // Insert logic for 'Mutating' state here
    }

    private void ApplyGlowing()
    {
        // Insert logic for 'Glowing' state here
    }

    private void ApplyDefensiveTerrain()
    {
        // Insert logic for 'DefensiveTerrain' state here
    }

    private void ApplyChlorophyllInfusion()
    {
        chlorophyllInfusionCounter += Time.deltaTime;
        
        if (chlorophyllInfusionDuration <= 0)
        {
            RemoveCondition(StatusConditions.statusList.ChlorophyllInfusion);
            chlorophyllInfusionCounter = 0;
        }

        if (chlorophyllInfusionCounter > 1 )
        {
            
            chlorophyllInfusionDuration -= chlorophyllInfusionCounter;
            chlorophyllInfusionCounter = 0;
            var hpBuildUp = 2 + TileMastery.Instance.masteryOverVelheret / 20;
            myStats.AddBonusHealth(hpBuildUp);
            
        }
    }
}
