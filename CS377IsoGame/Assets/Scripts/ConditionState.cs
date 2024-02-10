using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState : MonoBehaviour
{
    #region Components

    private StatsTemplate myStats;

    #endregion
    
    #region DebuffStatusFlags

    private bool burning;
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
    
    #endregion

    #region ExhaustedData

    private float exhaustedDuration;

    #endregion

    #region  RejuvenationData

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

    #endregion

    #region SlipperyStepsData

    private float slipperyStepsDuration;
    private int slipperyStepsIntensity;

    #endregion

    #region SmolderingStrikesData

    private float smolderingStrikesDuration;

    #endregion
    
    #region EvasiveData

    private float evasiveDuration;
    
    #endregion
    
    #region DefensiveTerrainData

    private float defensiveTerrainDuration;
    
    #endregion

    #region MutatingData

    private float mutatingDuration;

    #endregion

    #region RadiationPoisoningData

    private float radiationPoisoningDuration;

    #endregion

    #region GlowingData

    private float glowingDuration;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        activeSlows = new Slow[10];
        activeRejuvenations = new Rejuvenation[4];
    }

    // Update is called once per frame
    void Update()
    {
        if (burning)
        {
            ApplyBurning();
        }
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
/// </summary>
/// <param name="condition"></param>
/// <param name="duration"></param>
/// <param name="intensity"></param>
    public void SetCondition(StatusConditions.statusList condition, float duration = 0f, int intensity = 1)
    {
        switch (condition)
        {
            case StatusConditions.statusList.Burning:
                burning = true;
                if (duration > burningDuration) burningDuration = duration; //refresh duration
               /* if (!ongoingFX)
                {
                    
                }*/
                break;
            case StatusConditions.statusList.Corrosive: //intensity levels are 1, 2 and 3
                corrosive = true;
                if (duration >
                    corrosiveDuration) //doesn't stack, but if a higher intensity version is applied, it inherits the longer duration from among the two
                {
                    corrosiveDuration = duration;
                }

                if (intensity > corrosiveIntensity)
                {
                    corrosiveIntensity = intensity;
                }

                break;
            case StatusConditions.statusList.Bleeding:
                bleeding = true;
                bleedingStackCount++;
                if (duration > bleedingDuration)
                {
                    bleedingDuration = duration;
                }

                break;
            case StatusConditions.statusList.Confused:
                if (!glowing)
                {
                    confused = true;

                    if (duration > confusedDuration)
                    {
                        confusedDuration = duration;
                    }
                }

                break;
            case StatusConditions.statusList.Stunned:
                if (!glowing)
                {
                    stunned = true;
                    stunnedDuration = duration;
                }

                break;
            case StatusConditions.statusList.Slow: //intensity level between 1-18, corresponding to scaled multiples of 5%. 
                if (!glowing)
                {
                    
                    slowed = true; //this is an additive debuff, which means consecutive different intensity applications stack, 

                    if (activeSlows.Length <=
                        10) //however, only the current active debuff with the greatest intensity actually affects the player
                    {
                        activeSlows[activeSlows.Length - 1] = new Slow(intensity, duration);
                    }
                }

                break;
            case StatusConditions.statusList.Exhausted:
                exhausted = true;
                if(exhaustedDuration < duration) exhaustedDuration = duration;
                break;
            case StatusConditions.statusList.Rejuvenation: //intensity level between 1-4, works the same way as slow
                rejuvenation = true;
                if (activeRejuvenations.Length <=4)
                {
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
            CharacterHealth.Instance.TakeDamage(tickDamage);
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
