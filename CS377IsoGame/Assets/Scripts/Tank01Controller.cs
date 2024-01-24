using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank01Controller : MonoBehaviour
{
    [SerializeField] Transform TurretTransform;
    [SerializeField] Transform BarrelTransform;
    [SerializeField] GameObject TankMissilePrefab;

    public float TankRestHeight = 1.5f;          // target height position for tank
    public float TankControlFactor = 1f;         // controls how quickly the thrusters travel between their min and max thrust based on current tank height

    public float TurnRate = 1f;                  // modifier for turret and barrel quaternion slerp
    public float MinBarrelAngle = -7f;           // lowest angle tank barrel can be at

    public float TankMissileLaunchOffset = 5f;   // distance missile is launched from the front of the barrel
    public float TankMissileSpeed = 25f;         // launch speed of missile
    public float TankMissileFireDelay = 3f;      // minimum time between missile fire
    public float BarrelShootingAngleRange = 15f; // Tank shoots iff barrel is within X deg off vector to character posn
    private float TankMissileLastFireTime;

    private Rigidbody TankRigidbody;
    private Quaternion CurrentTurretAngles;
    private Quaternion CurrentBarrelAngles;

    // We could create an Enemy parent class to store basic enemy functionality...
    // This could be part of the Enemy parent class
    Transform PlayerTransform;
    public void GetPlayerTransform()
    {
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    void AimTurret()
    {
        Vector3 playerPosn = PlayerTransform.position;
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

    void MaybeFire()
    {
        Vector3 playerPosn = PlayerTransform.position;
        Vector3 toPlayer = playerPosn - BarrelTransform.position; // vector from barrel to player

        // Determine the left/right angle to the player from current turret orientation
        float turretAngle = Mathf.Abs(Vector3.SignedAngle(BarrelTransform.rotation * Vector3.forward, toPlayer, BarrelTransform.rotation * Vector3.up));

        if (turretAngle < BarrelShootingAngleRange && Time.time - TankMissileLastFireTime > TankMissileFireDelay)
        {
            // Create missile instance
            var Missile = Instantiate(
                TankMissilePrefab,
                BarrelTransform.position + BarrelTransform.forward * TankMissileLaunchOffset,
                BarrelTransform.rotation);

            // Set missile velocity
            var MissileRB = Missile.GetComponent<Rigidbody>();
            MissileRB.velocity = BarrelTransform.forward * TankMissileSpeed;

            // Add a "kick back" force at the end of the tank's barrel
            TankRigidbody.AddForceAtPosition(
                -BarrelTransform.forward * TankMissileSpeed * MissileRB.mass, // force
                BarrelTransform.position - 2 * BarrelTransform.forward, // apply force to end of barrel
                ForceMode.Impulse); // we want an instantaneous "pulse" to be applied

            TankMissileLastFireTime = Time.time;
        }
    }

    void Start()
    {
        TankRigidbody = GetComponent<Rigidbody>();
        GetPlayerTransform();
        TankMissileLastFireTime = Time.time;
    }

    void Update()
    {
        //base.Update();
        AimTurret();
    }

    void FixedUpdate()
    {
        MaybeFire();
    }
}
