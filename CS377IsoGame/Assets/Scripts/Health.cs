using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public StatsTemplate myStats;
    public float currentHealth;
    public float maxHealth;
    [FormerlySerializedAs("mySoul")] public EnemyAI myEnemyAI;
    public bool amIEnemy;
    public DamageEffect damageComponent;
    
    
    [SerializeField] public UnityEngine.UI.Slider healthSlider;

    private UnityAction<float> UpdateCharacterHealthListener;

    void Start()
    {
        myEnemyAI = GetComponent<EnemyAI>();
        damageComponent = GetComponentInChildren<DamageEffect>();
        if (myEnemyAI != null)
        {
            amIEnemy = true;
            OnDisable();
        }
        maxHealth = myStats.ceHP;
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        if (!amIEnemy)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void RestoreHealth(float heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
    }

    public void UpdateMaxHealth(float newMaxHealth)
    {
        float scalingFactor = maxHealth / newMaxHealth;
        currentHealth *= scalingFactor;
        maxHealth = newMaxHealth;
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
