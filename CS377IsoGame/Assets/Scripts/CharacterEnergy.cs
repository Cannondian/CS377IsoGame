using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnergy : Singleton<CharacterEnergy>
{
    private float currentEnergy;
    public static Action UltimateIsReady;
    private void Start()
    {
        currentEnergy = 0;
        OnEnable();
    }

    private void Update()
    {
        if (currentEnergy >= 100)
        {
            UltimateIsReady?.Invoke();
        } 
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
