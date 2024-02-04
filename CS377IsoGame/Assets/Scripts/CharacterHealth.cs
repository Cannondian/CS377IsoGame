using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterHealth : Singleton<CharacterHealth>
{
    public float currentHealth;
    public float maxHealth;

    public Slider healthSlider;

    private UnityAction<EventTypes.Event3Param> UpdateCharacterHealthListener;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthSlider.value = 100f * currentHealth / maxHealth;
    }

    public void TakeDamage(EventTypes.Event3Param context)
    {
        Debug.Log("Triggered! Taking damage...");
        currentHealth -= context.damageToTake;
    }

    private void OnEnable()
    {
        UpdateCharacterHealthListener += TakeDamage;
        EventBus.StartListening(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, UpdateCharacterHealthListener);
    }

    private void OnDisable()
    {
        EventBus.StopListening(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, UpdateCharacterHealthListener);
    }
}
