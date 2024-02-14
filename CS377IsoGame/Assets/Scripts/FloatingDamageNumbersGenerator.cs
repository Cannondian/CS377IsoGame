using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class FloatingDamageNumbers : Singleton<FloatingDamageNumbers>
{
    // Start is called before the first frame update

    public GameObject damageTextPrefab;
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
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(context.target.transform.position);
        
        var damageText = Instantiate(damageTextPrefab, screenPosition, 
            quaternion.identity, canvas.transform);
        var text = damageText.GetComponent<TextMeshProUGUI>();
        damageText.transform.position = RandomOffset(damageText.transform.position);
        text.text = context.damage.ToString();
        
        
        Destroy(damageText, 1.25f);
    }

    private Vector3 RandomOffset(Vector3 oldPosition)
    {
        float randomX = UnityEngine.Random.Range(-60, 60);
        float randomY = UnityEngine.Random.Range(50, 150);
        var newPosition = oldPosition + new Vector3(randomX, randomY, 0);
        return newPosition;
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
