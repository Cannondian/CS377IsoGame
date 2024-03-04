using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileElement : MonoBehaviour
{
    #region Brainstorm

    //how do we design the tile system? Tiles have a component called tileElement, but since these components
    //exist for all tiles, it might slow down the system to have this component do the work to apply status on entities
    //we should have a component on the entities check for element tiles, and if they are standing on one, a component should apply
    //status conditions as they are appropriate

    #endregion
    
    
    
    public enum ElementType {Zulzara,Velheret,Ilsihre,Shalharan,Obhalas,Dust,Radioactive, None}
    [SerializeField] public ElementType elementType;

    public Renderer tileRender;
    // Start is called before the first frame update
    void Start()
    {
        tileRender = GetComponent<Renderer>();
    }

    // Update is called once per frame
    
    void Update()
    {
        //good design to have this in the update, this way we can change tile in response to player actions,
        //that being said, maybe we can have a switch statement?
        if (elementType == ElementType.Zulzara)
        {
            //tileRender.material.color = Color.black;
        }
        else if (elementType == ElementType.Velheret)
        {
            //tileRender.material.color = Color.green;
        }
        else if (elementType == ElementType.Ilsihre)
        {
            
        }
        else if (elementType == ElementType.Shalharan)
        {
            //tileRender.material.color = Color.white;
        }
        else if (elementType == ElementType.Obhalas)
        {
            //tileRender.material.color = Color.cyan;
        }
        else if (elementType == ElementType.Dust)
        {
           // tileRender.material.color = Color.magenta;
        }
        else if (elementType == ElementType.Radioactive)
        {
            //tileRender.material.color = Color.yellow;
        }
    }
    
}

