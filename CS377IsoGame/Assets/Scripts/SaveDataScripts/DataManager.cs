using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // note that data is assigned to player in stats template
    private void OnApplicationQuit()
    {
        SaveSystem.ClearPlayerStats();
    }

}
