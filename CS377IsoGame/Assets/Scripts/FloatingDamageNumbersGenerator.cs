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
    public GameObject poisonDamageTextPrefab;
    public GameObject healingTextPrefab;
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
                textSizeScale = 20f;
                break;
            case 2:
                textSizeScale = 28f;
                break;
            case 3:
                textSizeScale = 36f;
                break;
            case 4:
                textSizeScale = 40F;
                break;
        }

        GameObject textToUse = genericDamageTextPrefab;
        switch (context.firstType)
        {
            case Damage.Types.Fire:
                textToUse = fireDamageTextPrefab;
                break;
            case Damage.Types.Poison:
                textToUse = poisonDamageTextPrefab;
                break;
            case Damage.Types.Healing:
                textToUse = healingTextPrefab;
                break;
                
        }
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(context.target.transform.position);
        Debug.Log("I am trying");
        var damageText = Instantiate(textToUse, screenPosition, 
            quaternion.identity, canvas.transform);
        
        var text = damageText.GetComponent<TextMeshProUGUI>();
        text.fontSize = textSizeScale;
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
                    text2.fontSize = textSizeScale / 3;
                    break;
                    
            }
        }
        Destroy(damageText, 1.2f);
        //StayOnTarget(context.target, damageText, 1.2f);
    }

    private IEnumerator StayOnTarget(GameObject target, GameObject text, float duration)
    {

        if (duration <= 0)
        {
            Destroy(text);
        }
        else
        {
            text.transform.position = RandomOffset(Camera.main.WorldToScreenPoint(target.transform.position), false);

            yield return StayOnTarget(target, text, duration - Time.deltaTime);
        }
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
            float randomX = UnityEngine.Random.Range(40, 50);
            float randomY = UnityEngine.Random.Range(-30, 30);
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
