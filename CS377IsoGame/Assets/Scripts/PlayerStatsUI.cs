using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Slider defenseSlider;
    public Slider attackSlider;
    public Slider speedSlider;
    //public Slider RRRSlider;
    public Slider attackSpeedSlider;
    //public Slider evasivenessSlider;

    public StatsTemplate playerStats;

    #region CoreCharge

    public GameObject jarBar;
    public Transform playerTransform;
    public Slider coreChargeSlider;

    #endregion

    #region Health

    public Health myHealth;
    public Slider healthSlider;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCoreChargeSlider();
        UpdateHealthSlider();
        
        defenseSlider.value = playerStats.tDefense;
        attackSlider.value = playerStats.tAttack;
        speedSlider.value = playerStats.tSpeed;
        
        //RRRSlider.value = playerStats.tRRR;
        //attackSpeedSlider.value = playerStats.tAttackSpeed;
        //evasivenessSlider.value = playerStats.tEvasiveness;
    }

    public void UpdateCoreChargeSlider()
    {
        var playerPos =  Camera.main.WorldToScreenPoint(playerTransform.position);
        jarBar.transform.position = playerPos + new Vector3(60, -40, 0);
        coreChargeSlider.value = CoreChargeManager.Instance.coreChargeState;
    }

    public void UpdateHealthSlider()
    {
        healthSlider.maxValue = myHealth.maxHealth;
        healthSlider.value = myHealth.currentHealth ;
    }
}
