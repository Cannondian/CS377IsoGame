using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Slider defenseSlider;
    public Slider attackSlider;
    public Slider speedSlider;
    //public Slider RRRSlider;
    public Slider attackSpeedSlider;
    //public Slider evasivenessSlider;

    
    
    public GameObject SmolderingStrikesIcon;
    public GameObject ChlorophyllInfusionIcon;
    public GameObject RejuvinationIcon;
    public GameObject SlipperyStepsIcon;
    public GameObject TailwindBoostIcon;
  
    private RollManager rollManager;
    public StatsTemplate playerStats;
    public ConditionState playerState;
    
    #region CoreCharge

    public GameObject jarBar;
    public Transform playerTransform;
    public Slider coreChargeSlider;

    #endregion

    #region Health

   
    public Slider healthSlider;
    public Slider bonusHealthSlider;

    #endregion
    
    #region Roll

    public GameObject rollBulb1;
    public GameObject rollBulb2;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        rollManager = GetComponent<RollManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCoreChargeSlider();
        UpdateHealthSlider();
        DrawBonusHealth();
        UpdateRollIndicator();
        
        
        
        //RRRSlider.value = playerStats.tRRR;
        //attackSpeedSlider.value = playerStats.tAttackSpeed;
        //evasivenessSlider.value = playerStats.tEvasiveness;
    }

    public void UpdateCoreChargeSlider()
    {
        var playerPos =  Camera.main.WorldToScreenPoint(playerTransform.position);
        jarBar.transform.position = playerPos + new Vector3(-50, -70, 0);
        coreChargeSlider.value = CoreChargeManager.Instance.coreChargeState;
    }

    public void UpdateHealthSlider()
    {
        healthSlider.maxValue = playerStats.tHP;
        healthSlider.value = playerStats.myCurrentHealth;
    }

    public void DrawBonusHealth()
    {
        bonusHealthSlider.value = playerStats.bonusHealth;
    }

    public void UpdateRollIndicator()
    {
        if (rollManager.roll1Ready)
        {
            rollBulb1.SetActive(true);
            
        }
        else
        {
            rollBulb1.SetActive(false);
        }
        if (rollManager.roll2Ready)
        {
            rollBulb2.SetActive(true);
            
        }
        else
        {
            rollBulb2.SetActive(false);
        }
    }

    public void UpdateStatusDial(StatusConditions.statusList condition, bool active)
    {
        if (condition == StatusConditions.statusList.SmolderingStrikes)
        {
            SmolderingStrikesIcon.SetActive(active);
        }

        if (condition == StatusConditions.statusList.ChlorophyllInfusion)
        {
            ChlorophyllInfusionIcon.SetActive(active);
        }

        if (condition == StatusConditions.statusList.Rejuvenation)
        {
            RejuvinationIcon.SetActive(active);
        }

        if (condition == StatusConditions.statusList.SlipperySteps)
        {
            SlipperyStepsIcon.SetActive(active);
        }
    }
}
