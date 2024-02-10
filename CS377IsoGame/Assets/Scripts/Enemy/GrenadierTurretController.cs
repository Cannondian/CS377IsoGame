using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierTurretController : EnemyAI
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
    public float MissileMaxLifeSpan = 10f;

    private Quaternion CurrentTurretAngles;
    private Quaternion CurrentArmAngles;

    private float LastFireTime;
    private bool WhichBarrel = true; // use to switch between firing from left/right barrel
    private float LaunchVelocity;
    private float TimeToPlayer;

    protected override void Awake()
    {
        base.Awake();

        LastFireTime = Time.time;

        Health = 100f;
        AttackDamage = 10f;
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }

    protected override void ChasePlayer()
    {
        AimTurret();
    }

    protected override void AttackPlayer()
    {
        AimTurret();
        MaybeFire();
    }

    void AimTurret()
    {
        Vector3 playerPosn = player.position;
        Vector3 toPlayer = playerPosn - transform.position; // vector from turret to player

        // Determine the left/right angle to the player
        float turretAngle = Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);

        // Smooth rotation from current angle to desired angle
        CurrentTurretAngles = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, turretAngle, 0f), TurnRate * Time.deltaTime);
        transform.rotation = CurrentTurretAngles;

        // Determine barrel angle and launch velocity based on current horizontal distance to player
        // ~magic~ -3*offset seems to make it work close enough to account for height difference of launch point and player posn
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
        // If past cooldown, fire next grenade at player
        if (Time.time - LastFireTime > FireDelay)
        {
            // Create missile instance
            Transform Missile;

            // Determine which barrel to fire from
            if (WhichBarrel)
            {
                Missile = Instantiate(
                    MissilePrefab,
                    Barrel1Transform.position + Barrel1Transform.forward * MissileLaunchOffset,
                    Barrel1Transform.rotation);

                // TODO: barrel recoil animation
            }
            else
            {
                Missile = Instantiate(
                    MissilePrefab,
                    Barrel2Transform.position + Barrel2Transform.forward * MissileLaunchOffset,
                    Barrel2Transform.rotation);

                // TODO: barrel recoil animation
            }

            // Set missile velocity
            var MissileRB = Missile.GetComponent<Rigidbody>();
            MissileRB.velocity = Barrel1Transform.forward * LaunchVelocity;

            // Set missile damage and lifespan
            GrenadierMissile GM = Missile.GetComponent<GrenadierMissile>();
            GM.Damage = AttackDamage;
            // GM.MaxLifeSpan = MissileMaxLifeSpan; 

            LastFireTime = Time.time;
            WhichBarrel = !WhichBarrel; // Alternate barrel
        }
    }
}
