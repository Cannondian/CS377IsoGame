using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterEnergy : Singleton<CharacterEnergy>
{
    private UnityAction<float> UltimateIsUsedListener;
    private UnityAction<float> SkillIsUsedListener;
    private UnityAction<float> IncrementEenergyListener;

    public Slider skillSlider;
    public Slider ultimateSlider;
    
    private float ultimateEnergyState;
    private float skillEnergyState;
    private float skillEnergyNeed;
    private float ultimateEnergyNeed;
    
    public bool ultimateIsReady;
    public bool skillIsReady;

    public float energyFromBasic = 3;
    public float energyFromEnhancedBasic = 6;
    public float energyFromSkill = 15;
    public float energyFromKill = 5;
    public float energyFromHit = 1;
    public float energyModifier;
    
    private void Start()
    {
        skillEnergyNeed = 40;
        ultimateEnergyNeed = 120;
        ultimateEnergyState = 0;
        skillEnergyState = 40;
        energyModifier = 1;
        skillIsReady = true;
        UpdateSliders();
    }

    

    private void UltimateUsed(float ph)
    {
        
        ultimateIsReady = false;
        ultimateEnergyState = 0;
        UpdateSliders();
    }
    private void SkillUsed(float ph)
    {
        Debug.Log("used");
        skillIsReady = false;
        skillEnergyState = 0;
        UpdateSliders();
    }
    private void IncrementEnergy(float amount)
    {
        skillEnergyState += amount * energyModifier;
        ultimateEnergyState += amount * energyModifier;
        if (ultimateEnergyState >= ultimateEnergyNeed)
        {
            ultimateIsReady = true;
        }
        if (skillEnergyState >= skillEnergyNeed)
        {
            skillIsReady = true;
        }
        UpdateSliders();
        
    }

    private void UpdateSliders()
    {
        ultimateSlider.value = ultimateEnergyState;
        skillSlider.value = skillEnergyState;
    }
    private void OnEnable()
    {
        SkillIsUsedListener += SkillUsed;
        EventBus.StartListening(EventTypes.Events.ON_SKILL_USED, SkillIsUsedListener);
        UltimateIsUsedListener += UltimateUsed;
        EventBus.StartListening(EventTypes.Events.ON_ULTIMATE_USED, UltimateIsUsedListener);
        IncrementEenergyListener += IncrementEnergy;
        EventBus.StartListening(EventTypes.Events.ON_ENERGY_GAIN, IncrementEenergyListener);
    }

    private void OnDisable()
    {
        
    }
}
