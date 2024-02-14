using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] public Material originalMaterial;
    [SerializeField] public Color damageColor;
    [SerializeField] public Color backToNormal;

    private IEnumerator damageRoutine;
    private UnityAction<float> damageEffectListener;

    private void Start()
    {
        originalMaterial.color = backToNormal;
    }
    private void OnEnable()
    {
        damageEffectListener += OnDamage;
        EventBus.StartListening(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, damageEffectListener);
    }
    
    private void OnDisable()
    {
        EventBus.StopListening(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, damageEffectListener);
    }

    public void OnDamage(float damage)
    {
        if (damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
        }
        damageRoutine = ColorFlash(damage);
        StartCoroutine(damageRoutine);
    }
    
    private IEnumerator ColorFlash(float damage)
    {
         
         originalMaterial.color = damageColor;
        
        yield return new WaitForSeconds(1);
        originalMaterial.color = backToNormal;
    }
}
