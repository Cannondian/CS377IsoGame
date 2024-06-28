using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using UnityEngine;
using UnityEngine.Events;

public class TileEffects : MonoBehaviour
{
    private ConditionState myState;
    public TileElement.ElementType currentTile;
    public TileMastery myMastery;
    private bool player;
    
    public UnityEvent<TileElement.ElementType> tileAnnouncement;    
     
    // Start is called before the first frame update
    void Start()
    {
        myState = GetComponent<ConditionState>();
        if (GetComponent<RPGCharacterController>() != null)
        {
            player = true;
            myMastery = TileMastery.Instance;
        }

        currentTile = TileElement.ElementType.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        DetectTile();
        ApplyTileEffects();
    }

    public void DetectTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up * -1, out hit, 4, 1 << 11))
        {
            TileElement tile;
            TileElement.ElementType elementalTile;
            hit.collider.gameObject.TryGetComponent<TileElement>(out tile);
            if (tile == null)
            {
                elementalTile = TileElement.ElementType.None;
            }
            else
            {
                elementalTile = tile.elementType;
            }
            if (currentTile != elementalTile && player)
            {
                tileAnnouncement.Invoke(elementalTile);
                currentTile = elementalTile;
               
            }
        }
    }
    
    private void ApplyTileEffects()
    {
        if (currentTile != TileElement.ElementType.None)
        {
            float intensity = 1;
            switch (currentTile)
            {
                
                case TileElement.ElementType.Velheret:
                    
                    myState.SetCondition(StatusConditions.statusList.ChlorophyllInfusion, 8, 1);
                    break;
                case TileElement.ElementType.Ilsihre:
                    if (player)
                    {
                        intensity = 1 + myMastery.IlsihreEffectIntensity();
                        myState.SetCondition(StatusConditions.statusList.SmolderingStrikes, 8, intensity);
                    }
                    else
                    {
                        myState.SetCondition(StatusConditions.statusList.Burning, 8, intensity);
                    }
                    
                    break;
                case TileElement.ElementType.Shalharan:
                    if (player)
                    {
                        intensity = 1 + myMastery.ShalharanEffectIntensity();
                    }
                    myState.SetCondition(StatusConditions.statusList.SlipperySteps, 8, intensity);
                    break;
                
            }
        }
    }
}