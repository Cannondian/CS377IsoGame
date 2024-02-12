using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitEffects : Singleton<PlayerHitEffects>
{
    
    public interface IHitEffect { }

    
    public class HitEffect<T1, T2> : IHitEffect
    {
        private float additionalDamage;

        public delegate void AdditionalEffect(T1 param1, T2 param2);

        private AdditionalEffect _additionalEffect;

        public HitEffect(float damage, AdditionalEffect method)
        {
            additionalDamage = damage;
            _additionalEffect = method;
        }

    }

    public List<IHitEffect> activeHitEffects;
    
    // Start is called before the first frame update
    void Start()
    {
        activeHitEffects = new List<IHitEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHitEffect(IHitEffect hitEffect)
    {
        activeHitEffects.Add(hitEffect);
    }

    public void RemoveHitEffect(IHitEffect hitEffect)
    {
        activeHitEffects.Remove(hitEffect);
    }
}
