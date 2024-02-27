using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileMastery : Singleton<TileMastery>
{

    #region Masteries

    public float masteryOverVelheret;
    public float masteryOverShalharan;
    public float masteryOverIlsihre;
    public float masteryOverZulzara;
    public float masteryOverObhalas;

    #endregion

    public TileElement.ElementType effectiveTile { get; set; }
   
    
    // Start is called before the first frame update
    void Start()
    {
        effectiveTile = TileElement.ElementType.None;
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
