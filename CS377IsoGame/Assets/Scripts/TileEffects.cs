using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffects : MonoBehaviour
{
    private ConditionState myState;
    public TileElement currentTile;
     
    // Start is called before the first frame update
    void Start()
    {
        myState = GetComponent<ConditionState>();
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
            switch (currentTile.elementType)
            {
                case TileElement.ElementType.Mech:
                    myState.SetCondition(StatusConditions.statusList.Hacked);
                    break;
                case TileElement.ElementType.Plant:
                    myState.SetCondition(StatusConditions.statusList.Rejuvenation);
                    break;
                case TileElement.ElementType.Fire:
                    myState.SetCondition(StatusConditions.statusList.Burning, 8);
                    myState.SetCondition(StatusConditions.statusList.SmolderingStrikes, 8);
                    break;
                case TileElement.ElementType.Ice:
                    myState.SetCondition(StatusConditions.statusList.Slow);
                    break;
                case TileElement.ElementType.Blob:
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