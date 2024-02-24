using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpingGrenadierController : EnemyAI
{
    [SerializeField] Transform BodyTransform;
    [SerializeField] Transform TurretTransform; // I.e., the "head" of the enemy where grenades are launched from
    [SerializeField] Transform TurretBottomTransform;
    [SerializeField] LineRenderer LineToIndicator; // A line from the enemy to its indicator to help show which enemy is attacking the player
    [SerializeField] Animator _Animator;
    [SerializeField] NavMeshAgent Agent;

    [SerializeField] GameObject GrenadePrefab;
    [SerializeField] GameObject MuzzleFX;
    [SerializeField] Transform Indicator;

    public float IndicatorSpeed; // How quickly the target indicator can follow the player
    public int BurstCount; // How many grenades are shot in quick succession for each time the enemy fires
    public float BurstInterval; // Time between grenades within a single burst
    public float CooldownTime; // Time between burst shot fires and jumps when patrolling
    public float VerticalLaunchVelocity;
    public float MaxFireDistanceToPlayer; // Maximum allowable distance between indicator and player position to enable firing
    public float JumpProbabilityWhenAttacking; // Percent change to jump instead of aiming for the player when in attack range
    public float MinJumpDistance; // Minimum distance of jumps when searching for walkpoints by player
    public float MaxJumpDistance;
    
    public bool WaitForLastGrenadeToLand;
    public bool EnableLaunchVariation;
    public float LaunchVariationMaximum; // Max amount of launch velocity variation as a function of distance to player

    private Vector3 LaunchOffset = new Vector3(0f, 1.1f, 0f); // Offset from turret origin where grenade is fired
    private float LastBurstFireTime;
    private float LastPatrolJumpTime;
    private bool Attacking; // Indicates if currently in firing sequence
    private bool WillAttack; // Used in preventing the grenadier from jumping once its decided whether to jump or attack

    private bool IsIndicatorOn;

    protected override void Awake()
    {
        base.Awake();

        Indicator.gameObject.SetActive(false);
        ResetIndicator();

        // prevent this enemy from attacking/jumping right away
        LastBurstFireTime = Time.time;
        LastPatrolJumpTime = Time.time;

        Health = 50f;
        AttackDamage = 5f;
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

        // The -75f is to keep the green bar facing the indicator
        TurretTransform.rotation = Quaternion.Euler(0f, turretAngle - 75f, 0f);

        // Make the bottom section to spin opposite the "head" (if we want stationary, set it to Quaternion.identity)
        TurretBottomTransform.rotation = Quaternion.Inverse(TurretTransform.rotation);

        UpdateLineToIndicator();
    }

    void MaybeFire()
    {
        if (Time.time - LastBurstFireTime > CooldownTime // past cooldown time
            && (player.position - Indicator.position).magnitude < MaxFireDistanceToPlayer) // indicator within attack range
        {
            // Attacking sequence potentially started, so set WillAttack to false
            WillAttack = false;

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

            SoundManager.PlaySound(SoundManager.Sound.Grenadier_Fire, transform.position);

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
            yield return new WaitForSeconds(BurstInterval/2f);
            DisableIndicator();
            yield return new WaitForSeconds(BurstInterval/2f);
            EnableIndicator();
        }

        if (WaitForLastGrenadeToLand)
        {
            // wait until last grenade hits ground before aiming again
            yield return new WaitForSeconds(TimeToGround);
        }

        Attacking = false;
        LastBurstFireTime = Time.time;
    }

    protected override void Patroling()
    {
        if (!Attacking && !walkPointSet && Time.time - LastPatrolJumpTime > CooldownTime)
        {
            LastPatrolJumpTime = Time.time;

            ResetIndicator();
            DisableIndicator();
            SearchForPatrollingJumpLocation();
        }
    }

    protected override void ChasePlayer()
    {
        if (!Attacking && !walkPointSet)
        {
            ResetIndicator();
            DisableIndicator();
            SearchForJumpLocationByPlayer();
        }
    }

    protected override void AttackPlayer()
    {
        if (!walkPointSet && !Attacking)
        {
            // Potentially jump to new position instead of attacking
            if (Random.Range(0f, 1f) < JumpProbabilityWhenAttacking && !WillAttack)
            {
                WillAttack = false;
                SearchForJumpLocationByPlayer();
            }
            else
            {
                WillAttack = true;
                MaybeFire();
            }
        }
    }

    void SearchForJumpLocationByPlayer()
    {
        int i = 0;
        while(!walkPointSet && i < 100)
        {
            // Similar to SearchWalkPoint but walkPoint is now centered around player and within attack range
            walkPoint = new Vector3(player.position.x + Random.Range(-attackRange, attackRange), 
                                    player.position.y, 
                                    player.position.z + Random.Range(-attackRange, attackRange));
            
            float JumpDistance = (walkPoint - transform.position).magnitude;

            if (Physics.Raycast(walkPoint, -transform.up, 2f, Walkable) 
                && JumpDistance > MinJumpDistance 
                && JumpDistance < MaxJumpDistance)
            {
                walkPointSet = true;
            }

            i += 1;
        }

        if (walkPointSet)
        {
            StartCoroutine(JumpToWalkPoint());    
        }
    }

    void SearchForPatrollingJumpLocation()
    {
        int i = 0;
        while(!walkPointSet && i < 100)
        {
            walkPoint = new Vector3(transform.position.x + Random.Range(-attackRange, attackRange), 
                                    transform.position.y, 
                                    transform.position.z + Random.Range(-attackRange, attackRange));
            
            float JumpDistance = (walkPoint - transform.position).magnitude;

            if (Physics.Raycast(walkPoint, -transform.up, 2f, Walkable) 
                && JumpDistance > MinJumpDistance 
                && JumpDistance < MaxJumpDistance)
            {
                walkPointSet = true;
            }

            i += 1;
        }

        if (walkPointSet)
        {
            StartCoroutine(JumpToWalkPoint());    
        }
    }

    IEnumerator JumpToWalkPoint()
    {
        // Turn off indicator for duration of jump if it would already be on
        if (playerInSightRange || playerInAttackRange)
        {
            DisableIndicator();
        }

        // Determine distance to target location and the time it will take to get there
        float DistanceToTarget = (transform.position - walkPoint).magnitude;
        float TimeToTarget = DistanceToTarget / Agent.speed;

        // Adjust animation speed so that jump animation spans the time to jump
        _Animator.enabled = true;
        _Animator.speed = 1f / (TimeToTarget * 1.25f); // scale up time to target to account for startup and stop sections of animation
        _Animator.Play("JumpingGrenadierJump");

        // Wait for first 7/60th of animation (when grenadier is priming its jump)
        yield return new WaitForSeconds(_Animator.speed * 7f/60f);
        
        // Move the enemy to the walkpoint
        agent.SetDestination(walkPoint);

        // Wait for animation to complete
        yield return new WaitForSeconds(TimeToTarget + _Animator.speed * 8f/60f);

        // Stop animation, ensure we reset it as well (so the enemy does not end up floating)
        walkPointSet = false;
        _Animator.Rebind();
        _Animator.Update(0f);
        _Animator.enabled = false;

        // Turn indicator back on (if it should be)
        ResetIndicator();
        if (playerInSightRange || playerInAttackRange)
        {
            EnableIndicator();
        }

        walkPointSet = false;
    }

    void ResetIndicator()
    {
        // Set indicator position to current enemy position
        Indicator.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        UpdateLineToIndicator();
    }

    void EnableIndicator()
    {
        if (!IsIndicatorOn)
        {            
            Indicator.gameObject.SetActive(true);
            LineToIndicator.enabled = true;
            IsIndicatorOn = true;
        }
    }

    void DisableIndicator()
    {
        if (IsIndicatorOn)
        {
            Indicator.gameObject.SetActive(false);
            LineToIndicator.enabled = false;
            IsIndicatorOn = false;
        }
    }

    void UpdateLineToIndicator()
    {
        LineToIndicator.SetPosition(0, new Vector3(transform.position.x, Indicator.position.y, transform.position.z));
        LineToIndicator.SetPosition(1, Indicator.position);
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
