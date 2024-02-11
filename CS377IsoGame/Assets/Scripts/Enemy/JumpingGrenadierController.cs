using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingGrenadierController : EnemyAI
{
    [SerializeField] Transform BodyTransform;
    [SerializeField] Transform TurretTransform; // I.e., the "head" of the enemy where grenades are launched from
    [SerializeField] Transform TurretBottomTransform;
    [SerializeField] Animator _Animator;

    [SerializeField] GameObject GrenadePrefab;
    [SerializeField] GameObject MuzzleFX;
    [SerializeField] Transform Indicator;

    public float IndicatorSpeed; // How quickly the target indicator can follow the player
    public int BurstCount; // How many grenades are shot in quick succession for each time the enemy fires
    public float BurstInterval; // Time between grenades within a single burst
    public float CooldownTime; // Time between burst shot fires
    public float VerticalLaunchVelocity;
    public float MaxFireDistanceToPlayer; // Maximum allowable distance between indicator and player position to enable firing
    public float JumpProbabilityWhenFiring;
    
    public bool EnableLaunchVariation;
    public float LaunchVariationMaximum; // Max amount of launch velocity variation as a function of distance to player

    private Vector3 LaunchOffset = new Vector3(0f, 1.1f, 0f); // Offset from turret origin where grenade is fired
    private float LastBurstFireTime;
    private bool Attacking; // Indicates if currently in firing sequence

    private bool IsIndicatorOn;

    protected override void Awake()
    {
        base.Awake();

        Indicator.gameObject.SetActive(false);
        ResetIndicator();

        // prevent this enemy from attacking right away
        LastBurstFireTime = Time.time;

        Health = 100f;
        AttackDamage = 10f;
    }

    void Update()
    {
        // We put aiming here to enable smoother tracking for the indicator
        if (IsIndicatorOn && !Attacking) 
        {
            AimTurret();
        }
    }

    void AimTurret()
    {
        // Smoothly interpolate indicator position to current player position
        Indicator.position = new Vector3(
            Mathf.Lerp(Indicator.position.x, player.position.x, IndicatorSpeed * Time.deltaTime),
            player.position.y,
            Mathf.Lerp(Indicator.position.z, player.position.z, IndicatorSpeed * Time.deltaTime)
        );

        // Scale indicator size with distance from this enemy to show decrease in accuracy
        float HorizontalDistanceFromBase = new Vector2(Indicator.position.x - transform.position.x, Indicator.position.z - transform.position.z).magnitude;
        float IndicatorScale = Mathf.Clamp(40f * LaunchVariationMaximum * HorizontalDistanceFromBase / sightRange, 0.5f, 4f) * 2f;
        Indicator.localScale = new Vector3(IndicatorScale, IndicatorScale, IndicatorScale);

        // Determine the left/right angle to the indicator
        float turretAngle = Vector3.SignedAngle(Vector3.forward, Indicator.position - transform.position, Vector3.up);

        // The -60f is to keep the green bar facing the indicator
        TurretTransform.rotation = Quaternion.Euler(0f, turretAngle - 60f, 0f);

        // Make the bottom section to spin opposite the "head" (if we want stationary, set it to Quaternion.identity)
        TurretBottomTransform.rotation = Quaternion.Inverse(TurretTransform.rotation);
    }

    void MaybeFire()
    {
        if (!walkPointSet // not moving
            && !Attacking // not attacking
            && Time.time - LastBurstFireTime > CooldownTime // past cooldown time
            && (player.position - Indicator.position).magnitude < MaxFireDistanceToPlayer) // within attack range
        {
            // Potentially jump to new position instead of attacking
            if (Random.Range(0f, 1f) < JumpProbabilityWhenFiring)
            {
                walkPoint = new Vector3(player.position.x + Random.Range(-attackRange, attackRange), 
                                        player.position.y, 
                                        player.position.z + Random.Range(-attackRange, attackRange));
                agent.SetDestination(walkPoint);
                walkPointSet = true;
            }
            else
            {
                Attacking = true;
                StartCoroutine(BurstFire());
            }
        }
    }

    IEnumerator BurstFire() 
    {
        float TimeToGround = 0f;
        for (int i = 0; i < BurstCount; i++)
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
            TimeToGround = TimeToApex + Mathf.Sqrt(2f * (Grenade.transform.position.y + VerticalLaunchVelocity * TimeToApex) / -Physics.gravity.y);
            float HorizontalDistanceFromBase = new Vector2(Indicator.position.x - transform.position.x, Indicator.position.z - transform.position.z).magnitude;
            float HorizontalLaunchVelocity = (HorizontalDistanceFromBase / TimeToGround) + HorizontalDistanceFromBase * 0.05f; // last part is a fudge factor to make sure targeting is perfect

            // Potentially add variation in horizontal launch velocity
            float LaunchVariationX = 0f;
            float LaunchVariationZ = 0f;
            if (EnableLaunchVariation)
            {
                LaunchVariationX = Random.Range(-LaunchVariationMaximum, LaunchVariationMaximum) * HorizontalDistanceFromBase;
                LaunchVariationZ = Random.Range(-LaunchVariationMaximum, LaunchVariationMaximum) * HorizontalDistanceFromBase;
            }

            GrenadeRB.velocity = new Vector3(
                HorizontalLaunchVelocity * (Indicator.position.x - transform.position.x) / HorizontalDistanceFromBase + LaunchVariationX,
                VerticalLaunchVelocity,
                HorizontalLaunchVelocity * (Indicator.position.z - transform.position.z) / HorizontalDistanceFromBase + LaunchVariationZ
            );

            var settings = Grenade.GetComponent<GrenadierMissile>();
            settings.Damage = AttackDamage;
            // Todo: any more settings for grenade?

            // Blink indicator, wait before firing next grenade
            DisableIndicator();
            yield return new WaitForSeconds(BurstInterval);
            EnableIndicator();
        }

        // wait until last grenade hits ground before aiming again
        yield return new WaitForSeconds(TimeToGround);

        Attacking = false;
        LastBurstFireTime = Time.time;
    }

    protected override void Patroling()
    {
        if (!Attacking)
        {
            base.Patroling();

            ResetIndicator();
            DisableIndicator();
        }
    }

    protected override void ChasePlayer()
    {
        if (!Attacking && !walkPointSet)
        {
            ResetIndicator();
            DisableIndicator();

            // Move to some where within attack range of player
            walkPoint = new Vector3(player.position.x + Random.Range(-attackRange, attackRange), 
                                    player.position.y, 
                                    player.position.z + Random.Range(-attackRange, attackRange));
            agent.SetDestination(walkPoint);
            walkPointSet = true;
        }

        if ((transform.position - walkPoint).magnitude < 1f)
            walkPointSet = false;
    }

    protected override void AttackPlayer()
    {
        if ((transform.position - walkPoint).magnitude < 1f)
            walkPointSet = false;

        EnableIndicator();
        MaybeFire();
    }

    void ResetIndicator()
    {
        // Set indicator position to current enemy position
        Indicator.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
    }

    void EnableIndicator()
    {
        if (!IsIndicatorOn)
        {            
            Indicator.gameObject.SetActive(true);
            IsIndicatorOn = true;
        }
    }

    void DisableIndicator()
    {
        if (IsIndicatorOn)
        {
            Indicator.gameObject.SetActive(false);
            IsIndicatorOn = false;
        }
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
