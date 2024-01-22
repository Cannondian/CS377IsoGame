using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    //components
    private Transform playerTransform;
    public static CameraHandler singleton;

    
    //variables
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    
    //constants
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    private Vector3 offsetVector;
    
    // Start is called before the first frame update
    void Start()
    {
        
        singleton = this;
        playerTransform = FindObjectOfType(typeof(RPGCharacterController)).GameObject().transform;
        offsetVector = new Vector3(offsetX, offsetY, offsetZ);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        SetRotation();
    }

    void FollowPlayer()
    {
        targetPosition = new Vector3(playerTransform.position.x + offsetX,
            playerTransform.position.y + offsetY,
            playerTransform.position.z + offsetZ);
        transform.position = targetPosition;
    }

    void SetRotation()
    {
        transform.LookAt(playerTransform);
    }
}
