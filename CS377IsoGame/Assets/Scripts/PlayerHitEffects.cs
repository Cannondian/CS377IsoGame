using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitEffects : Singleton<PlayerHitEffects>
{
    
    public interface IHitEffect { }

    
    public class HitEffect : IHitEffect
    {
        public float additionalDamage;

        public delegate void AdditionalEffect(GameObject self, GameObject other);

        public AdditionalEffect _additionalEffect;

        public HitEffect(float damage, AdditionalEffect method)
        {
            additionalDamage = damage;
            _additionalEffect = method;
        }

    }

    public List<HitEffect> activeHitEffects;
    
    // Start is called before the first frame update
    void Start()
    {
        activeHitEffects = new List<HitEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHitEffect(HitEffect hitEffect)
    {
        activeHitEffects.Add(hitEffect);
    }

    public void RemoveHitEffect(HitEffect hitEffect)
    {
        activeHitEffects.Remove(hitEffect);
    }

    public bool AnyHitEffects()
    {
        if (activeHitEffects.Count != 0) return true;
        return false;
    }
}
