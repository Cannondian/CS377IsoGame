using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterEnergy : Singleton<CharacterEnergy>
{
    private UnityAction<EnergySpenders> ContinousEnergyDrainListener;
    private UnityAction<EnergySpenders> SkillIsUsedListener;
    private UnityAction<float> IncrementEenergyListener;

    [SerializeField] public Slider energySlider;
    [SerializeField] public Slider ultimateSlider;
    [SerializeField] public StatsTemplate playerStats;
    
    
   private float energyState;
    
    public bool ultimateIsReady;
    public bool skillIsReady;

    public float energyFromBasic = 3;
    public float energyFromEnhancedBasic = 6;
    public float energyFromSkill = 15;
    public float energyFromKill = 5;
    public float energyFromHit = 1;
    public float energyModifier;

    public float drain = -3;
    public bool draining;
    
    public enum EnergySpenders
    {
        Flamethrower = 3,
        Mine = 60,
        AttunementChip = 100,
    }
    
    private void Start()
    {

        energyModifier = 1;
        skillIsReady = true;
        UpdateSliders();
    }

    private void Update()
    {
        DrainEnergy();
        

    }

    private void DrainEnergy()
    {
        if (draining && energyState >= 0)
        {
            energyState -= drain;
        }
    }

    private void SetDrain(EnergySpenders culprit)
    {
        
        draining = true;
        drain = (int)culprit;

    }

    private void SpendEnergy(EnergySpenders skill)
    {
        
    }

    public bool SkillAvailable(EnergySpenders skill)
    {
        return energyState >= (float)skill;
    }
    private void IncrementEnergy(float amount)
    {
        energyState = amount * energyModifier;
        UpdateSliders();
        
    }

    private void UpdateSliders()
    {
        energySlider.value = energyState;
    }
    private void OnEnable()
    {
        SkillIsUsedListener += SpendEnergy;
        EventBus.StartListening(EventTypes.Events.ON_SKILL_USED, SkillIsUsedListener);
        ContinousEnergyDrainListener += SetDrain;
        EventBus.StartListening(EventTypes.Events.ON_CONTINOUS_ENERGY_DRAIN_START, ContinousEnergyDrainListener);
        IncrementEenergyListener += IncrementEnergy;
        EventBus.StartListening(EventTypes.Events.ON_ENERGY_GAIN, IncrementEenergyListener);
    }

    private void OnDisable()
    {
        
    }
}
