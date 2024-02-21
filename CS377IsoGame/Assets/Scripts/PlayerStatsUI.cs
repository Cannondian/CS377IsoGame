using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Slider defenseSlider;
    public Slider attackSlider;
    public Slider speedSlider;
    public Slider RRRSlider;
    public Slider attackSpeedSlider;
    public Slider evasivenessSlider;

    public StatsTemplate playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        defenseSlider.value = playerStats.tDefense;
        attackSlider.value = playerStats.tAttack;
        speedSlider.value = playerStats.tSpeed;
        //RRRSlider.value = playerStats.tRRR;
        //attackSpeedSlider.value = playerStats.tAttackSpeed;
        //evasivenessSlider.value = playerStats.tEvasiveness;
    }
}
