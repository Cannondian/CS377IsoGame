using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Lookups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class PlayerGameStateTracker : Singleton<PlayerGameStateTracker>
{
    public UnityEvent onRecalculateStats;
    public Texture2D sciFiCursor;
    private bool cursorSet;
    public GameObject player;
    float playerHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (!cursorSet)
        {
            Cursor.SetCursor(sciFiCursor, new Vector2(0, 0), CursorMode.Auto);
            Cursor.visible = true;
            cursorSet = true;
        }

        playerHealth = player.GetComponent<StatsTemplate>().myCurrentHealth;
        // Debug.Log("current hp" + playerHealth);
        if (playerHealth < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
