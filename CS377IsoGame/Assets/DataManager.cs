using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // note that data is assigned to player in stats template
    private void OnApplicationQuit()
    {
        SaveSystem.ClearPlayerStats();
    }

}
