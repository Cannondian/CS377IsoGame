using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingGrenadierController : EnemyAI
{
    [SerializeField] Transform TurretTransform; // I.e., the "head" of the enemy where grenades are launched from
    [SerializeField] Transform TurretBottomTransform;

    [SerializeField] GameObject GrenadePrefab;
    [SerializeField] GameObject MuzzleFX;

    public float TurnRate; // How quickly the head rotates to track the player
    public int BurstCount; // How many grenades are shot in quick succession for each time the enemy fires
    public float BurstInterval; // Time between grenades within a single burst
    public float CooldownTime; // Time between burst shot fires
    public float VerticalLaunchVelocity;
    
    public bool EnableLaunchVariation;
    public float LaunchVariationMaximum; // Max amount of launch variation as a function of distance to player

    private Vector3 LaunchOffset = new Vector3(0f, 1.1f, 0f); // Offset from turret origin where grenade is fired
    private float LastBurstFireTime;
    private bool Attacking; // Indicates if currently in firing sequence
    private Vector3 ToPlayer;

    protected override void Awake()
    {
        base.Awake();

        // prevent this enemy from attacking right away
        LastBurstFireTime = Time.time;

        Health = 100f;
        AttackDamage = 10f;
    }

    void AimTurret()
    {
        ToPlayer = player.position - TurretTransform.position; // vector from turret to player

        // Determine the left/right angle to the player
        float turretAngle = Vector3.SignedAngle(Vector3.forward, ToPlayer, Vector3.up);

        // Smooth rotation from current angle to desired angle
        // The -60f is to keep the green indicator facing the player
        TurretTransform.rotation = Quaternion.Slerp(TurretTransform.rotation, Quaternion.Euler(0f, turretAngle - 60f, 0f), TurnRate * Time.deltaTime);

        // If we want the bottom section to spin opposite the "head"
        TurretBottomTransform.rotation = Quaternion.Inverse(TurretTransform.rotation);
        
        // If we want the bottom section to remain stationary relative to the body
        // TurretBottomTransform.rotation = Quaternion.identity;
    }

    void MaybeFire()
    {
        if (Time.time - LastBurstFireTime > CooldownTime && !Attacking)
        {
            Attacking = true;
            StartCoroutine(BurstFire());
        }
    }

    IEnumerator BurstFire() 
    {
        var FireInstanceToPlayer = player.position - TurretTransform.position;

        int i = 0;
        while (i < BurstCount)
        {
            // Firing effect at end of barrel
            Destroy(
                Instantiate(
                    MuzzleFX, 
                    TurretTransform.position + LaunchOffset, 
                    Quaternion.identity
                ),
                1f // Destroy FX after 1 second
            );

            var Grenade = Instantiate(
                GrenadePrefab,
                TurretTransform.position + LaunchOffset,
                Quaternion.identity
            );
            Rigidbody GrenadeRB = Grenade.GetComponent<Rigidbody>();

            // Determine launch velocity
            float TimeToApex = VerticalLaunchVelocity / -Physics.gravity.y;
            float TimeToGround = TimeToApex + Mathf.Sqrt(2f * (Grenade.transform.position.y + VerticalLaunchVelocity * TimeToApex) / -Physics.gravity.y);
            float HorizontalDistanceToPlayer = new Vector2(FireInstanceToPlayer.x, FireInstanceToPlayer.z).magnitude;
            float HorizontalLaunchVelocity = (HorizontalDistanceToPlayer / TimeToGround) + HorizontalDistanceToPlayer * 0.05f; // last part is a fudge factor to make sure targeting is perfect

            float LaunchVariationX = 0f;
            float LaunchVariationZ = 0f;
            if (EnableLaunchVariation)
            {
                LaunchVariationX = Random.Range(-LaunchVariationMaximum, LaunchVariationMaximum) * HorizontalDistanceToPlayer;
                LaunchVariationZ = Random.Range(-LaunchVariationMaximum, LaunchVariationMaximum) * HorizontalDistanceToPlayer;
            }

            GrenadeRB.velocity = new Vector3(
                HorizontalLaunchVelocity * FireInstanceToPlayer.x / HorizontalDistanceToPlayer + LaunchVariationX,
                VerticalLaunchVelocity,
                HorizontalLaunchVelocity * FireInstanceToPlayer.z / HorizontalDistanceToPlayer + LaunchVariationZ
            );

            var settings = Grenade.GetComponent<GrenadierMissile>();
            settings.Damage = AttackDamage;
            // Todo: any more settings?

            i += 1;
            yield return new WaitForSeconds(BurstInterval);
        }

        Attacking = false;
        LastBurstFireTime = Time.time;
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

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
