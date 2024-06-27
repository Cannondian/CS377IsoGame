using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TileMastery : Singleton<TileMastery>
{
    public UnityEvent<float> OnShalharanChange;
    public UnityAction OnVelheretMod2;
    #region Masteries

    public float masteryOverVelheret;
    [FormerlySerializedAs("masteryOverShalharan")] public float _masteryOverShalharan;
    public float masteryOverIlsihre;
    public float masteryOverZulzara;
    public float masteryOverObhalas;

    public float masteryOverShalharan
    {
        get { return _masteryOverShalharan; }

        set
        {
            if (_masteryOverShalharan != value)
            {
                _masteryOverShalharan = value;
                if (_ShalharanMod2)
                {
                    OnShalharanChange.Invoke(value);
                }
            }
        }
    }
    #endregion
    
    #region TileEffectEnhancements

    public bool IlsihreMod1;
    [FormerlySerializedAs("_IlsihreMod2")] public bool IlsihreMod2;
    public bool VelheretMod1;
    public bool _VelheretMod2;
    public bool ShalharanMod1;
    [FormerlySerializedAs("ShalharanMod2")] public bool _ShalharanMod2;
    public List<ModifierVault.IModifier> appliedTEE;

    public bool ShalharanMod2
    {
        get { return _ShalharanMod2; }
        set
        {
            if (_ShalharanMod2 != value)
            {
                _ShalharanMod2 = value;
                if (_ShalharanMod2)
                {
                    OnShalharanChange.Invoke(masteryOverShalharan);
                }
            }
        }
    }

    public bool VelheretMod2
    {
        get { return _VelheretMod2; }
        set
        {
            if (_VelheretMod2 != value)
            {
                _VelheretMod2 = value;
                if (_VelheretMod2)
                {
                    OnVelheretMod2.Invoke();
                }
            }
        }
    }
    
    #endregion

    public TileElement.ElementType effectiveTile { get; set; }
   
    
    // Start is called before the first frame update
    void Start()
    {
        effectiveTile = TileElement.ElementType.None;
        TileMasteryData data = SaveSystem.LoadTileMastery();
        if (data != null)
        {
            masteryOverVelheret = data.masteryOverVelheret;
            _masteryOverShalharan = data.masteryOverShalharan;
            masteryOverIlsihre = data.masteryOverIlsihre;
            IlsihreMod1 = data.IlsihreMod1;
            IlsihreMod2 = data.IlsihreMod2;
            VelheretMod1 = data.VelheretMod1;
            VelheretMod2 = data.VelheretMod2;
            ShalharanMod1 = data.ShalharanMod1;
            _ShalharanMod2 = data.ShalharanMod2;
            appliedTEE = data.TEEList;
            
            foreach (ModifierVault.TileEffectEnhancement mod in appliedTEE)
            {
                mod.ApplyModifier();
            }
        }
        else
        {
            appliedTEE = new List<ModifierVault.IModifier>();
        }
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
        return _masteryOverShalharan * 0.1f;
    }

    

    
    
}
