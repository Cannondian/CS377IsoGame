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
    
    
    
    public enum ElementType {Velheret,Ilsihre,Shalharan,None}
    [SerializeField] public ElementType elementType;

    public Renderer tileRender;
    // Start is called before the first frame update
    void Start()
    {
        tileRender = GetComponent<Renderer>();
    }

    // Update is called once per frame
    
    
    
}

