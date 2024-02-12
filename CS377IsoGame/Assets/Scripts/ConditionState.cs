using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using UnityEditor.Rendering;
using UnityEngine;

public class ConditionState : MonoBehaviour
{
   
    public Health mySoul; //health component
    
    #region Components

    private StatsTemplate myStats;
    private FXHandler FXhandler;

    #endregion
    
    #region DebuffStatusFlags

    public bool burning;
    private bool corrosive;
    private bool bleeding;
    private bool confused;
    private bool stunned;
    private bool slowed;
    private bool exhausted;
    private bool blinded;
    private bool hacked;
    private bool radiationPoisoning;
   

    #endregion
    

    #region BuffStatusFlags

    private bool rejuvenation;
    private bool energized;
    private bool slipperySteps;
    private bool smolderingStrikes;
    private bool evasive;
    private bool oneWithTheWorld;
    private bool unstoppable;
    private bool mutating;
    private bool glowing;
    private bool defensiveTerrain;

    #endregion
    
    #region JisaStatusFlags
    
    
    
    #endregion
    
    #region BurningData

    private float burningDuration;
    

    #endregion

    #region CorrosiveData

    private float corrosiveDuration;
    private int corrosiveIntensity;
    private StatModifier corrosiveModifier;

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

    #region SlowedData

    public StatModifier slowModifier; //stat modifier that represents all active slow effect on the entity
    public struct Slow
    {
        public readonly int slowIntensity;
        public readonly float slowDuration;
        
        public Slow(int i, float d)
        {
            slowIntensity = i;
            slowDuration = d;
        }
    }

    private Slow[] activeSlows;
    private float effectiveSlowIntensity;
    
    #endregion

    #region ExhaustedData

    private float exhaustedDuration;
    private StatModifier exhaustedModifier;

    #endregion

    #region  RejuvenationData

    public int effectiveRejuvenationIntensityforFX;
    public struct Rejuvenation
    {
        public float duration;
        public int intensity;

        public Rejuvenation(float d, int i)
        {
            duration = d;
            intensity = i;
        }

    }

    private Rejuvenation[] activeRejuvenations;

    #endregion

    #region EnergizedData

    private float energizedDuration;
    private StatModifier energizedModifier;

    #endregion

    #region SlipperyStepsData

    private float slipperyStepsDuration;
    private int slipperyStepsIntensity;
    private StatModifier slipperyStepsModifier; 

    #endregion

    #region SmolderingStrikesData

    private float smolderingStrikesDuration;
    private StatModifier smolderingStrikesModifier;

    #endregion
    
    #region EvasiveData

    private float evasiveDuration;
    private StatModifier evasiveModifier;
    
    #endregion
    
    #region DefensiveTerrainData

    private float defensiveTerrainDuration;
    private StatModifier defensiveTerrainModifier;
    
    #endregion

    #region MutatingData

    private float mutatingDuration;
    private StatModifier mutatingModifier1;
    private StatModifier mutatingModifier2;

    #endregion

    #region RadiationPoisoningData

    private float radiationPoisoningDuration;
    private StatModifier radiationPoisoningModifier1;
    private StatModifier radiationPoisoningModifier2;

    #endregion

    #region GlowingData

    private float glowingDuration;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        activeSlows = new Slow[10];
        activeRejuvenations = new Rejuvenation[4];
        myStats = GetComponent<StatsTemplate>();
        mySoul = GetComponent<Health>();
        FXhandler = FXHandler.Instance;
        
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

        if (corrosive)
        {
            ApplyCorrosive();
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
    public void SetCondition(StatusConditions.statusList condition, float duration = 0f, int intensity = 1)
    {
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
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION, 
                        new EventTypes.StatusConditionFXParam(FXHandler.Instance.BurningFXPrefab, gameObject,  nameof(StatusConditions.statusList.Burning)));
                    burningDuration = duration;
                }
                //ui trigger will go here
                
               
                break;
            case StatusConditions.statusList.Corrosive: //intensity levels are 1, 2 and 3

                if (corrosiveIntensity < intensity)
                {
                    if (corrosive) 
                    { 
                        EventBus.TriggerEvent(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION, 
                        new EventTypes.StatusConditionFXParam(FXhandler.CorrosiveFXPrefab, gameObject,
                            nameof(StatusConditions.statusList.Corrosive)));
                    }
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.CorrosiveFXPrefab, gameObject,
                                                                nameof(StatusConditions.statusList.Corrosive), intensity));
                    
                    corrosiveModifier = new StatModifier(intensity * 20, StatModifierType.Percent);
                    myStats.AddModifier(StatsTemplate.statsList.Defense, corrosiveModifier);
                }

                corrosive = true;
                if (duration > corrosiveDuration)
                {
                    corrosiveDuration = duration;
                }

                break;
            case StatusConditions.statusList.Bleeding:
                if (!bleeding)
                {
                    bleeding = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                        new EventTypes.StatusConditionFXParam(FXhandler.BleedingFXPrefab,
                            gameObject, nameof(StatusConditions.statusList.Bleeding)));
                }

                if (bleedingStackCount < 10) bleedingStackCount++;
                
                if (duration > bleedingDuration)
                {
                    bleedingDuration = duration;
                }
                break;
            case StatusConditions.statusList.Confused:
                if (!glowing)
                {
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
                }

                break;
            case StatusConditions.statusList.Stunned:
                if (!glowing)
                {
                    if (!stunned)
                    {
                        stunned = true;
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.StunnedFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.Stunned)));
                    }
                    if (duration > stunnedDuration) stunnedDuration = duration;
                }

                break;
            case StatusConditions.statusList.Slow: //intensity level between 1-18, corresponding to scaled multiples of 5%. 
                if (!glowing)                       //modifiers get updated the effective slow value, which is the one with the highest intensity, FX are constant for all levels of intensity
                {
                    if (!slowed)
                    {
                        slowed = true;
                        EventBus.TriggerEvent(EventTypes.Events.ON_NEW_STATUS_CONDITION,
                            new EventTypes.StatusConditionFXParam(FXhandler.SlowFXPrefab, gameObject,
                                nameof(StatusConditions.statusList.Slow)));
                        activeSlows[0] = new Slow(intensity, duration);
                        slowModifier = new StatModifier(intensity * -5, StatModifierType.Percent);
                        myStats.AddModifier(StatsTemplate.statsList.Speed, slowModifier);
                    }
                    //this is an additive debuff, which means consecutive different intensity applications stack, 
                    if (activeSlows.Length <= 10) //however, only the current active debuff with the greatest intensity actually affects the player
                    {
                        activeSlows[activeSlows.Length - 1] = new Slow(intensity, duration);
                        if (effectiveSlowIntensity < intensity)
                        {
                            effectiveSlowIntensity = intensity;
                            myStats.RemoveModifier(StatsTemplate.statsList.Speed, slowModifier);
                            slowModifier = new StatModifier(intensity * -5, StatModifierType.Percent);
                            myStats.AddModifier(StatsTemplate.statsList.Speed, slowModifier);
                        }
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
                }
                if (activeRejuvenations.Length == 4)
                {
                    Rejuvenation rejuvenationToRemove;
                    int lowest;
                    foreach (Rejuvenation r in activeRejuvenations)
                    {
                        
                    }
                    activeRejuvenations[activeRejuvenations.Length - 1] = new Rejuvenation(duration, intensity);
                }
                break;
            case StatusConditions.statusList.Energized:
                energized = true;
                if (energizedDuration < duration) energizedDuration = duration;
                break;
            case StatusConditions.statusList.SlipperySteps: //buff with the highest intensity is used. Duration is constant for every application and is always reset
                slipperySteps = true;                       //intensity between 1-3;
                                                            //at intensity 3, also increases attack speed
                if (intensity > slipperyStepsIntensity)
                {
                    slipperyStepsIntensity = intensity;
                    slipperyStepsDuration = duration;
                }
                else
                {
                    slipperyStepsDuration = duration;
                }
                break;
            case StatusConditions.statusList.SmolderingStrikes: //scales with attack but has fixed intensity
                smolderingStrikes = true;
                if (smolderingStrikesDuration < duration)
                {
                    smolderingStrikesDuration = duration;
                }
                break;
            case StatusConditions.statusList.Evasive:
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
                radiationPoisoning = true;
                if (radiationPoisoningDuration < duration) radiationPoisoningDuration = duration;
                break;
            case StatusConditions.statusList.Mutating:
                mutating = true;
                if (mutatingDuration < duration) mutatingDuration = duration;
                break;
            case StatusConditions.statusList.Glowing:
                glowing = true;
                if (glowingDuration < duration) glowingDuration = duration;
                break;
            case StatusConditions.statusList.DefensiveTerrain: //fixed intensity, scales with defense
                defensiveTerrain = true;
                if (defensiveTerrainDuration < duration) defensiveTerrainDuration = duration;
                break;
        }
    }

    public void GetCondition(StatusConditions.statusList condition)
    {
        
    }

    public void RemoveCondition(StatusConditions.statusList condition)
    {
        
    }

    private void ApplyBurning()
    {
        burningDuration =- Time.deltaTime;
        if (burningDuration <= 0)
        {
            RemoveCondition(StatusConditions.statusList.Burning);
        }

        if (burningDuration % 1 == 0)
        {
            var tickDamage = myStats.tHP * 0.02f;
            mySoul.TakeDamage(tickDamage);
        }
       
    }

    private void ApplyCorrosive()
    {
        // Insert logic for 'Corrosive' state here
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
        // Insert logic for 'Rejuvenation' state here
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
        // Insert logic for 'SmolderingStrikes' state here
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
}
