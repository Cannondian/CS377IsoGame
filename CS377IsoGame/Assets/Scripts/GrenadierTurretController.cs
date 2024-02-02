using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierTurretController : MonoBehaviour
{

    [SerializeField] Transform Arm1Transform;
    [SerializeField] Transform Arm2Transform;
    [SerializeField] Transform Barrel1Transform;
    [SerializeField] Transform Barrel2Transform;
    [SerializeField] Transform MissilePrefab;

    public float TurnRate = 10f;
    public float MissileLaunchOffset = 2f;

    public float HorizontalMissileVelocity = 10f;
    public float FireDelay = 3f;

    private Rigidbody Barrel1RB;
    private Rigidbody Barrel2RB;
    private Vector3 Barrel1InitPosn;
    private Vector3 Barrel2InitPosn;

    private Quaternion CurrentTurretAngles;
    private Quaternion CurrentArmAngles;

    private float LastFireTime;
    private bool WhichBarrel = true; // use to switch between firing from left/right barrel
    private float LaunchVelocity;
    private float TimeToPlayer;

    Transform PlayerTransform;
    public void GetPlayerTransform()
    {
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Similar to tank's aim turret but with a different shooting method
    void AimTurret()
    {
        Vector3 playerPosn = PlayerTransform.position;
        Vector3 toPlayer = playerPosn - transform.position; // vector from turret to player

        // Determine the left/right angle to the player
        float turretAngle = Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);

        // Smooth rotation from current angle to desired angle
        CurrentTurretAngles = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, turretAngle, 0f), TurnRate * Time.deltaTime);
        transform.rotation = CurrentTurretAngles;

        float HorizontalDistanceToPlayer = new Vector2(toPlayer.x, toPlayer.z).magnitude - 3f * MissileLaunchOffset;
        TimeToPlayer = HorizontalDistanceToPlayer / HorizontalMissileVelocity;
        float InitialVerticalMissileVelocity = 0.5f * Physics.gravity.y * TimeToPlayer;
        float barrelAngle = Mathf.Atan2(InitialVerticalMissileVelocity, HorizontalMissileVelocity) * Mathf.Rad2Deg;

        LaunchVelocity = new Vector2(HorizontalMissileVelocity, InitialVerticalMissileVelocity).magnitude;

        CurrentArmAngles = Quaternion.Slerp(Arm1Transform.localRotation, Quaternion.Euler(barrelAngle, 0f, 0f), TurnRate * Time.deltaTime);
        Arm1Transform.localRotation = CurrentArmAngles;
        Arm2Transform.localRotation = CurrentArmAngles;
    }

    void MaybeFire()
    {

        if (Time.time - LastFireTime > FireDelay)
        {
            // Create missile instance
            Transform Missile;
            if (WhichBarrel)
            {
                Missile = Instantiate(
                    MissilePrefab,
                    Barrel1Transform.position + Barrel1Transform.forward * MissileLaunchOffset,
                    Barrel1Transform.rotation);

                Barrel1RB.AddForce(
                    -Barrel1Transform.forward,
                    ForceMode.Impulse);
            }
            else
            {
                Missile = Instantiate(
                    MissilePrefab,
                    Barrel2Transform.position + Barrel2Transform.forward * MissileLaunchOffset,
                    Barrel2Transform.rotation);

                Barrel2RB.AddForce(
                    -Barrel2Transform.forward,
                    ForceMode.Impulse);
            }

            // Set missile velocity
            var MissileRB = Missile.GetComponent<Rigidbody>();
            MissileRB.velocity = Barrel1Transform.forward * LaunchVelocity;

            

            LastFireTime = Time.time;
            WhichBarrel = !WhichBarrel;
        }
    }

    void LockBarrelsToLocal()
    {
        // Lock barrels to only move along local z axis
        Barrel1Transform.localPosition = new Vector3(Barrel1InitPosn.x, Barrel1InitPosn.y, Barrel1Transform.localPosition.z);
        Barrel2Transform.localPosition = new Vector3(Barrel2InitPosn.x, Barrel2InitPosn.y, Barrel2Transform.localPosition.z);
    }

    void Start()
    {
        LastFireTime = Time.time;
        GetPlayerTransform();

        Barrel1RB = Barrel1Transform.GetComponent<Rigidbody>();
        Barrel2RB = Barrel2Transform.GetComponent<Rigidbody>();

        Barrel1InitPosn = Barrel1Transform.localPosition;
        Barrel2InitPosn = Barrel2Transform.localPosition;
    }

    void Update()
    {
        AimTurret();
        LockBarrelsToLocal();
    }

    void FixedUpdate()
    {
        MaybeFire();
    }
}
