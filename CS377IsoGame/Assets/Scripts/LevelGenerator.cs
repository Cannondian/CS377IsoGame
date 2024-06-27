using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public int levelGenerate;
    public ModifierChoice chooseModifier;
    private StatsTemplate playerStats;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats = other.GetComponent<StatsTemplate>();
            ModifierChoice.Instance.ToggleChoiceBoxes(levelGenerate);
            ModifierChoice.Instance.modifierApplied.AddListener(LoadNextLevel);
        }
    }

    private void LoadNextLevel()
    {
        SaveSystem.SavePlayerStats(playerStats);
        SaveSystem.SaveTileMastery(TileMastery.Instance);
        Debug.Log("Loading Level " + levelGenerate);
        SceneManager.LoadScene(levelGenerate);
    }
}
