using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterHealth : Singleton<CharacterHealth>
{
    public float currentHealth;
    public float maxHealth;

    public UnityEngine.UI.Slider healthSlider;

    private UnityAction<float> UpdateCharacterHealthListener;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthSlider.value = currentHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
