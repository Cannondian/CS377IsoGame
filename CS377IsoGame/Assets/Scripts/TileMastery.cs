using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMastery : Singleton<TileMastery>
{

    #region Masteries

    public float masteryOverVelheret;
    public float masteryOverShalharan;
    public float masteryOverIlsihre;
    public float masteryOverZulzara;
    public float masteryOverObhalas;

    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float IlsihreEffectIntensity()
    {
        return masteryOverIlsihre * 0.1f;
    }

    public float ShalharanEffectIntensity()
    {
        return masteryOverShalharan * 0.1f;
    }
}
