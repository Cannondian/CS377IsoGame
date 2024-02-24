using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public int levelGenerate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveSystem.SavePlayerStats(other.GetComponent<StatsTemplate>());
            Debug.Log("Loading Level " + levelGenerate);
            SceneManager.LoadScene(levelGenerate);
        }
    }
}
