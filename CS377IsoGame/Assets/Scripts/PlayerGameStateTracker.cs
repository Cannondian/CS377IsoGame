using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Lookups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerGameStateTracker : Singleton<PlayerGameStateTracker>
{
    public UnityEvent onRecalculateStats;
    
    // Start is called before the first frame update
    void Start()
    {
        onRecalculateStats.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
