using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Lookups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerGameStateTracker : Singleton<PlayerGameStateTracker>
{
    public UnityEvent onRecalculateStats;
    public Texture2D sciFiCursor;
    private bool cursorSet;
    
    // Start is called before the first frame update
    void Start()
    {
        onRecalculateStats.Invoke();
        Cursor.SetCursor(sciFiCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!cursorSet)
        {
            Cursor.SetCursor(sciFiCursor, new Vector2(0, 0), CursorMode.Auto);
            Cursor.visible = true;
            cursorSet = true;
        }
    }
}
