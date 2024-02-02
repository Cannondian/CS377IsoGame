using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTankController : EnemyAI
{
    [SerializeField] Transform TurretTransform;
    [SerializeField] Transform BarrelTransform;
    [SerializeField] GameObject TankMissilePrefab;

    /// Tank hover controls no longer being used for this enemy
    //public float TankRestHeight = 1.5f;          // target height position for tank
    //public float TankControlFactor = 1f;         // controls how quickly the thrusters travel between their min and max thrust based on current tank height

    public float TurnRate = 1f;                  // modifier for turret and barrel quaternion slerp
    public float MinBarrelAngle = -7f;           // lowest angle tank barrel can be at

    public float TankMissileLaunchOffset = 5f;   // distance missile is launched from the front of the barrel (in local coords)
    public float TankMissileSpeed = 25f;         // launch speed of missile
    public float TankMissileFireDelay = 3f;      // minimum time between missile fire
    public float BarrelShootingAngleRange = 15f; // Tank shoots iff barrel is within X deg off vector to character posn
    private float TankMissileLastFireTime;

    //private Rigidbody TankRigidbody;
    private Quaternion CurrentTurretAngles;
    private Quaternion CurrentBarrelAngles;

    [SerializeField] private LineRenderer attackLineRenderer;
    [SerializeField] private float lineDisplayTime = 1f;

    private bool isFiring = false;

    protected override void Awake()
    {
        base.Awake();
        //TankRigidbody = GetComponent<Rigidbody>();
        TankMissileLastFireTime = Time.time;

        // Initialize tank-specific properties
        Health = 200f;
        AttackDamage = 30f; 
    }

    protected override void AttackPlayer()
    {
        // stop before attacking
        agent.SetDestination(transform.position); // this may be redundant now with stunned implementation

        if (!isFiring) { AimTurret(); }
        MaybeFire();
    }

    void AimTurret()
    {
        Vector3 playerPosn = player.position;
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
        Vector3 playerPosn = player.position;
        Vector3 toPlayer = playerPosn - BarrelTransform.position; // vector from barrel to player

        // Determine the left/right angle to the player from current turret orientation
        float turretAngle = Mathf.Abs(Vector3.SignedAngle(BarrelTransform.rotation * Vector3.forward, toPlayer, BarrelTransform.rotation * Vector3.up));

        if (turretAngle < BarrelShootingAngleRange && Time.time - TankMissileLastFireTime > TankMissileFireDelay && !isFiring)
        {
            StartCoroutine(FireSequence());
        }
    }

    private IEnumerator FireSequence()
    {
        isFiring = true;
        stunned = true; // tank cannot move during firing sequence

        // Show the attack line
        ShowAttackLine();

        // Wait for the specified delay time
        yield return new WaitForSeconds(lineDisplayTime);

        // Hide the attack line
        HideAttackLine();

        // Create missile instance
        var Missile = Instantiate(
            TankMissilePrefab,
            BarrelTransform.position + BarrelTransform.forward * TankMissileLaunchOffset,
            BarrelTransform.rotation);

        // Set missile velocity
        var MissileRB = Missile.GetComponent<Rigidbody>();
        MissileRB.velocity = BarrelTransform.forward * TankMissileSpeed;

        TankMissileLastFireTime = Time.time;

        stunned = false;
        isFiring = false;

        /* No longer being used for this enemy (no rigid body for now)
        // Add a "kick back" force at the end of the tank's barrel
        TankRigidbody.AddForceAtPosition(
            -BarrelTransform.forward * TankMissileSpeed * MissileRB.mass, // force
            BarrelTransform.position - 2 * BarrelTransform.forward, // apply force to end of barrel
            ForceMode.Impulse); // we want an instantaneous "pulse" to be applied
        TankMissileLastFireTime = Time.time;
        */

        // Wait for the remainder of the fire delay
        //yield return new WaitForSeconds(TankMissileFireDelay - lineDisplayTime);
        
    }

    private void ShowAttackLine()
    {
        if (attackLineRenderer != null)
        {
            Vector3 startPosn = BarrelTransform.position + BarrelTransform.forward * 1f;
            float distToPlayer = (player.position - startPosn).magnitude;
            attackLineRenderer.SetPosition(0, BarrelTransform.position + BarrelTransform.forward * 1f);
            attackLineRenderer.SetPosition(1, BarrelTransform.position + BarrelTransform.forward * distToPlayer);

            attackLineRenderer.enabled = true;

            Invoke("HideAttackLine", lineDisplayTime);
        }
    }

    private void HideAttackLine()
    {
        if (attackLineRenderer != null)
        {
            attackLineRenderer.enabled = false;
        }
    }
}
