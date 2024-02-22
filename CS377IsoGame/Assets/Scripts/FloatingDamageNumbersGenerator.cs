using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FloatingDamageNumbers : Singleton<FloatingDamageNumbers>
{
    // Start is called before the first frame update

    public GameObject genericDamageTextPrefab;
    public GameObject fireDamageTextPrefab;
    private UnityAction<EventTypes.FloatingDamageParam> floatingDamageListener;
    private GameObject canvas;
    
    private void OnEnable()
    {
        floatingDamageListener += ShowFloatingDamage;
        EventBus.StartListening(EventTypes.Events.ON_ENEMY_HIT, floatingDamageListener);
    }
    private void OnDisable()
    {
        EventBus.StopListening(EventTypes.Events.ON_ENEMY_HIT, floatingDamageListener);
    }
    private void ShowFloatingDamage(EventTypes.FloatingDamageParam context)
    {
        float textSizeScale = 0;
        switch (context.size)
        {
            case 1:
                textSizeScale = 1f;
                break;
            case 2:
                textSizeScale = 1.5f;
                break;
            case 3:
                textSizeScale = 1.8f;
                break;
            case 4:
                textSizeScale = 2.2f;
                break;
        }
        
        
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(context.target.transform.position);
        Debug.Log("I am trying");
        var damageText = Instantiate(genericDamageTextPrefab, screenPosition, 
            quaternion.identity, canvas.transform);
        damageText.transform.localScale = new Vector3(textSizeScale, textSizeScale, textSizeScale);
        var text = damageText.GetComponent<TextMeshProUGUI>();
        damageText.transform.position = RandomOffset(damageText.transform.position, false);
        text.text = context.damage.ToString();

        if (context.additionalDamage != 0)
        {
            
            switch (context.secondType)
            {
                case Damage.Types.Fire:
                    var damageText2 = Instantiate(fireDamageTextPrefab, damageText.transform);
                    
                    damageText2.transform.position = RandomOffset(damageText2.transform.position, true);
                    var text2 = damageText2.GetComponent<TextMeshProUGUI>();
                    text2.text = context.additionalDamage.ToString();
                    break;
                    
            }
        }
        Destroy(damageText, 1.2f);
    }

    private Vector3 RandomOffset(Vector3 oldPosition, bool isAdditional)
    {
        if (!isAdditional)
        {
            float randomX = UnityEngine.Random.Range(-60, 60);

            float randomY = UnityEngine.Random.Range(50, 150);
            var newPosition = oldPosition + new Vector3(randomX, randomY, 0);
            return newPosition;
        }
        else
        {
            float randomX = UnityEngine.Random.Range(30, 50);
            float randomY = UnityEngine.Random.Range(-20, 20);
            var newPosition = oldPosition + new Vector3(randomX, randomY, 0);
            return newPosition;
        }
    }
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
