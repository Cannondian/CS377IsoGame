using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank01Controller : MonoBehaviour
{
    [SerializeField] Transform TurretTransform;
    [SerializeField] Transform BarrelTransform;

    [SerializeField] public float TurnRate;
    [SerializeField] public float MinBarrelAngle;

    private Quaternion CurrentTurretAngles;
    private Quaternion CurrentBarrelAngles;

    // We could create an Enemy parent class to store basic enemy functionality...
    // This could be part of the Enemy parent class
    GameObject Player;
    public void GetPlayer()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        GetPlayer();
    }

    void Update()
    {
        Vector3 playerPosn = Player.transform.position;
        Vector3 toPlayer = playerPosn - TurretTransform.position; // vector from turret to player

        // Determine the left/right angle to the player from current tank body orientation
        float turretAngle = Vector3.SignedAngle(transform.rotation * Vector3.forward, toPlayer, transform.rotation * Vector3.up);

        // Smooth rotation from current angle to desired angle
        CurrentTurretAngles = Quaternion.Slerp(TurretTransform.localRotation, Quaternion.Euler(0f, turretAngle, 0f), TurnRate * Time.deltaTime);
        TurretTransform.localRotation = CurrentTurretAngles;

        // Determine the up/down angle to the player from the current *turret* orientation
        float barrelAngle = Vector3.SignedAngle(TurretTransform.rotation * Vector3.forward, toPlayer, TurretTransform.rotation * Vector3.right);
        // Limit bottom angle of barrel (note down for this model's barrel is positive, hence the sign flips!)
        if (-barrelAngle < MinBarrelAngle) { barrelAngle = -MinBarrelAngle; }
        CurrentBarrelAngles = Quaternion.Slerp(BarrelTransform.localRotation, Quaternion.Euler(barrelAngle, 0f, 0f), TurnRate * Time.deltaTime);
        BarrelTransform.localRotation = CurrentBarrelAngles;
    }
}
