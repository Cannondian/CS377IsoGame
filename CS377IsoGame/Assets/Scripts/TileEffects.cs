using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using UnityEngine;

public class TileEffects : MonoBehaviour
{
    private ConditionState myState;
    public TileElement currentTile;
    public TileMastery myMastery;
    private bool player;
     
    // Start is called before the first frame update
    void Start()
    {
        myState = GetComponent<ConditionState>();
        if (GetComponent<RPGCharacterController>() != null)
        {
            player = true;
            myMastery = TileMastery.Instance;
        }

        
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
            hit.collider.gameObject.TryGetComponent<TileElement>(out tile);
            currentTile = tile;
        }
    }
    
    private void ApplyTileEffects()
    {
        if (currentTile != null)
        {
            float intensity = 1;
            switch (currentTile.elementType)
            {
                case TileElement.ElementType.Zulzara:
                    myState.SetCondition(StatusConditions.statusList.Hacked);
                    break;
                case TileElement.ElementType.Velheret:
                    myState.SetCondition(StatusConditions.statusList.Rejuvenation);
                    break;
                case TileElement.ElementType.Ilsihre:
                    if (player)
                    {
                        intensity = 1 + myMastery.IlsihreEffectIntensity();
                    }
                    myState.SetCondition(StatusConditions.statusList.Burning, 8, intensity);
                    myState.SetCondition(StatusConditions.statusList.SmolderingStrikes, 8, intensity);
                    break;
                case TileElement.ElementType.Shalharan:
                    if (player)
                    {
                        intensity = 1 + myMastery.ShalharanEffectIntensity();
                    }
                    myState.SetCondition(StatusConditions.statusList.SlipperySteps, 8, intensity);
                    break;
                case TileElement.ElementType.Obhalas:
                    myState.SetCondition(StatusConditions.statusList.Corrosive);
                    break;
                case TileElement.ElementType.Dust:
                    myState.SetCondition(StatusConditions.statusList.Blinded);
                    break;
                case TileElement.ElementType.Radioactive:
                    myState.SetCondition(StatusConditions.statusList.RadiationPoisoning);
                    break;
            }
        }
    }
}