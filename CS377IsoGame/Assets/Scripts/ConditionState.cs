using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState : MonoBehaviour
{
    #region Components

    

    #endregion
    
    #region DebuffStatusFlags

    private bool burning;
    private bool corrosive;
    private bool bleeding;
    private bool confused;
    private bool stunned;
    private bool slow;
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

    #endregion

    #region BurningData

    private float burningDuration;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                if (duration > burningDuration) burningDuration = duration;
                break;
            case StatusConditions.statusList.Corrosive:
                // handle Corrosive condition
                break;
            case StatusConditions.statusList.Bleeding:
                // handle Bleeding condition
                break;
            case StatusConditions.statusList.Confused:
                // handle Confused condition
                break;
            case StatusConditions.statusList.Stunned:
                // handle Stunned condition
                break;
            case StatusConditions.statusList.Slow:
                // handle Slow condition
                break;
            case StatusConditions.statusList.Exhausted:
                // handle Exhausted condition
                break;
            case StatusConditions.statusList.Rejuvenation:
                // handle Rejuvenation condition
                break;
            case StatusConditions.statusList.Energized:
                // handle Energized condition
                break;
            case StatusConditions.statusList.SlipperySteps:
                // handle SlipperySteps condition
                break;
            case StatusConditions.statusList.SmolderingStrikes:
                // handle SmolderingStrikes condition
                break;
            case StatusConditions.statusList.Evasive:
                // handle Evasive condition
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
                // handle RadiationPoisoning condition
                break;
            case StatusConditions.statusList.Mutating:
                // handle Mutating condition
                break;
            case StatusConditions.statusList.Glowing:
                // handle Glowing condition
                break;
            case StatusConditions.statusList.DefensiveTerrain:
                // handle Glowing condition
                break;
        }
    }

    public void GetCondition(StatusConditions.statusList condition)
    {
        
    }

    public void RemoveCondition(StatusConditions.statusList condition)
    {
        
    }

    public void ApplyConditionEffects()
    {
        
    }
}
