using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventBus : MonoBehaviour
{

    #region fields

    private static Hashtable eventStorage = new Hashtable();

    #endregion
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void StartListening<T>(EventTypes.Events eventName, UnityAction<T> listener)
    {
        UnityEvent<T> thisEvent = null;
        string b = GetKey<T>(eventName);
        if (eventStorage.ContainsKey(b))
        {
            thisEvent = (UnityEvent<T>)eventStorage[b];
            thisEvent.AddListener(listener);
            eventStorage[eventName] = listener;
        }
        else
        {
            thisEvent = new UnityEvent<T>();
            thisEvent.AddListener(listener);
            eventStorage.Add(b, thisEvent);
        }
    }

    private static string GetKey<T>(EventTypes.Events eventName)
    {
        ;
        string key = typeof(T) + eventName.ToString();
        return key;
    }
    
    public static void StopListening<T>(EventTypes.Events eventName, UnityAction<T> listener)
    {
        string b = GetKey<T>(eventName);
        if (eventStorage.ContainsKey(b))
        {
            UnityEvent<T> thisEvent = (UnityEvent<T>)eventStorage[b];
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void TriggerEvent<T>(EventTypes.Events eventName, T param)
    {
        string b = GetKey<T>(eventName);
        if (eventStorage.ContainsKey(b))
        {
            UnityEvent<T> thisEvent = (UnityEvent<T>)eventStorage[b];
            thisEvent.Invoke(param);
        }
    }
    public static void StopListeningAll<T>()
    {
        string keyPrefix = typeof(T).ToString();
        List<string> keysToRemove = new List<string>();
        foreach (DictionaryEntry entry in eventStorage)
        {
            string key = (string)entry.Key;
            if (key.StartsWith(keyPrefix))
            {
                keysToRemove.Add(key);
            }
        }
        foreach (string key in keysToRemove)
        {
            eventStorage.Remove(key);
        }
    }
    
    
}