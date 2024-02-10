using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingGrenadierController : EnemyAI
{
    [SerializeField] Transform TurretTransform; // I.e., the "head" of the enemy where grenades are launched from
    [SerializeField] Transform TurretBottomTransform;

    [SerializeField] GameObject GrenadePrefab;
    [SerializeField] GameObject MuzzleFX;
    [SerializeField] Transform Indicator;

    public float IndicatorSpeed; // How quickly the target indicator can follow the player
    public int BurstCount; // How many grenades are shot in quick succession for each time the enemy fires
    public float BurstInterval; // Time between grenades within a single burst
    public float CooldownTime; // Time between burst shot fires
    public float VerticalLaunchVelocity;
    public float MaxFireDistanceToPlayer; // Maximum allowable distance between indicator and player position to enable firing
    
    public bool EnableLaunchVariation;
    public float LaunchVariationMaximum; // Max amount of launch velocity variation as a function of distance to player

    private Vector3 LaunchOffset = new Vector3(0f, 1.1f, 0f); // Offset from turret origin where grenade is fired
    private float LastBurstFireTime;
    private bool Attacking; // Indicates if currently in firing sequence
    private Vector3 ToPlayer;

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
        ToPlayer = player.position - TurretTransform.position; // vector from turret to player
        Indicator.position = new Vector3(
            Mathf.Lerp(Indicator.position.x, player.position.x, IndicatorSpeed * Time.deltaTime),
            player.position.y,
            Mathf.Lerp(Indicator.position.z, player.position.z, IndicatorSpeed * Time.deltaTime)
        );

        // Scale indicator size with distance to show decrease in accuracy
        float HorizontalDistanceToPlayer = new Vector2(Indicator.position.x, Indicator.position.z).magnitude;
        float IndicatorScale = Mathf.Clamp(40f * LaunchVariationMaximum * HorizontalDistanceToPlayer / sightRange, 0.5f, 4f);
        Indicator.localScale = new Vector3(IndicatorScale, IndicatorScale, IndicatorScale);

        // Determine the left/right angle to the indicator
        float turretAngle = Vector3.SignedAngle(Vector3.forward, Indicator.position, Vector3.up);

        // The -60f is to keep the green bar facing the indicator
        TurretTransform.rotation = Quaternion.Euler(0f, turretAngle - 60f, 0f);

        // Make the bottom section to spin opposite the "head" (if we want stationary, set it to Quaternion.identity)
        TurretBottomTransform.rotation = Quaternion.Inverse(TurretTransform.rotation);
    }

    void MaybeFire()
    {
        if (Time.time - LastBurstFireTime > CooldownTime
            && !Attacking 
            && (player.position - Indicator.position).magnitude < MaxFireDistanceToPlayer)
        {
            Attacking = true;
            StartCoroutine(BurstFire());
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
            float HorizontalDistanceToPlayer = new Vector2(Indicator.position.x, Indicator.position.z).magnitude;
            float HorizontalLaunchVelocity = (HorizontalDistanceToPlayer / TimeToGround) + HorizontalDistanceToPlayer * 0.05f; // last part is a fudge factor to make sure targeting is perfect

            // Potentially add variation in horizontal launch velocity
            float LaunchVariationX = 0f;
            float LaunchVariationZ = 0f;
            if (EnableLaunchVariation)
            {
                LaunchVariationX = Random.Range(-LaunchVariationMaximum, LaunchVariationMaximum) * HorizontalDistanceToPlayer;
                LaunchVariationZ = Random.Range(-LaunchVariationMaximum, LaunchVariationMaximum) * HorizontalDistanceToPlayer;
            }

            GrenadeRB.velocity = new Vector3(
                HorizontalLaunchVelocity * Indicator.position.x / HorizontalDistanceToPlayer + LaunchVariationX,
                VerticalLaunchVelocity,
                HorizontalLaunchVelocity * Indicator.position.z / HorizontalDistanceToPlayer + LaunchVariationZ
            );

            var settings = Grenade.GetComponent<GrenadierMissile>();
            settings.Damage = AttackDamage;
            // Todo: any more settings?

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
        ResetIndicator();
        DisableIndicator();
    }

    protected override void ChasePlayer()
    {
        EnableIndicator();
    }

    protected override void AttackPlayer()
    {
        EnableIndicator();
        MaybeFire();
    }

    void ResetIndicator()
    {
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
