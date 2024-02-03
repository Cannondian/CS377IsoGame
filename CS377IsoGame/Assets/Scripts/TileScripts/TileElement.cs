using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileElement : MonoBehaviour
{
    enum ElementType {Mech,Plant,Fire,Ice,Blob,Dust,Radioactive}
    [SerializeField] private ElementType elementType;

    public Renderer tileRender;
    // Start is called before the first frame update
    void Start()
    {
        tileRender = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (elementType == ElementType.Mech)
        {
            tileRender.material.color = Color.black;
        }
        else if (elementType == ElementType.Plant)
        {
            tileRender.material.color = Color.green;
        }
        else if (elementType == ElementType.Fire)
        {
            tileRender.material.color = Color.red;
        }
        else if (elementType == ElementType.Ice)
        {
            tileRender.material.color = Color.cyan;
        }
        else if (elementType == ElementType.Blob)
        {
            tileRender.material.color = Color.white;
        }
        else if (elementType == ElementType.Dust)
        {
            tileRender.material.color = Color.magenta;
        }
        else if (elementType == ElementType.Radioactive)
        {
            tileRender.material.color = Color.yellow;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            if (elementType == ElementType.Mech)
            {
                //
            }
            
        }
    }
}

