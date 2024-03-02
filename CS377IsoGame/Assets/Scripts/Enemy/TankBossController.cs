using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankBossController : EnemyAI
{
    [SerializeField] Transform TurretTransform;
    [SerializeField] Transform BarrelTransform;
    [SerializeField] Transform HatchTransform;
    [SerializeField] GameObject TankMissilePrefab;
    [SerializeField] GameObject MeleeDronePrefab;
    [SerializeField] GameObject AttackIndicator;
    [SerializeField] Rigidbody TankRigidbody;

    [SerializeField] GameObject TargetingCircle;
    [SerializeField] GameObject LaserCircle;

    private ParticleSystem TargetingParticles;
    private ParticleSystem LaserStrikeParticles;

    //// Note: these values are NOT the actual values saved to the prefab! Those are specified separately within the editor 
    //// (though these should be a good start in case something goes wrong)
    public float TankRestHeight = 1.5f;          // target height position for tank
    public float TankControlFactor = 1f;         // controls how quickly the thrusters travel between their min and max thrust based on current tank height

    public float TurnRate = 1f;                  // modifier for turret and barrel quaternion slerp
    public float MinBarrelAngle = -7f;           // lowest angle tank barrel can be at

    public float TankMissileLaunchOffset = 2f;   // distance missile is launched from the front of the barrel (in local coords)
    public float BarrelAngleOffset = 15f;        // Adds offset amount extra angle to barrel, allowing to shoot higher than necessary and give the missile more air time.
    public float TankMissileSpeed = 25f;         // launch speed of missile
    public float TankMissileFireDelay = 3f;      // minimum time between missile fire
    public float BarrelShootingAngleRange = 15f; // Tank shoots iff barrel is within X deg off vector to character posn
    public float TankMissileHomingStrength = 1f; // How strong the homing effect of the tank missile is (set to 0 to turn off homing)
    public float TankMissileMaxLifeSpan = 5f;    // The max lifespan of missiles before they auto-explode
    public float RepositionChance = 0.2f;        // Probability the tank will move away from the player on any given attack cycle
    public float LaserStrikeChance = 0.5f;       // Probability of performing the defensive laser strike if player is within range
    public float LaserStrikeActivationRange = 5f;// Distance Player has to be within for laser strike to be allowed and the distance out to which lasers can deal damage
    public float LaserStrikeDamage = 0.1f;       // Damage laser strike does per fixed update
    public float DroneSpawnChance = 0.2f;        // Probability of spawning a drone on any given attack cycle
    public float TankImpactDamage = 3f;          // Damage player takes if the tank collides with them
    
    private float TankMissileLastFireTime;
    private float LastWalkPointSetTime;
    private bool LasersActive;

    private Quaternion CurrentTurretAngles;
    private Quaternion CurrentBarrelAngles;

    // Attack indicator fields
    [SerializeField] private float AttackIndicatorDisplayTime = 1f;

    public bool isFiring = false;
    public bool willFire = false;

    protected override void Awake()
    {
        base.Awake();
        TankMissileLastFireTime = Time.time;
        AttackIndicator.SetActive(false);

        // Initialize tank-specific properties
        // Health = 200f;
        AttackDamage = 30f; 

        TargetingParticles = TargetingCircle.GetComponent<ParticleSystem>();
        LaserStrikeParticles = LaserCircle.GetComponent<ParticleSystem>();

        SoundManager.PlaySoundLoop(SoundManager.Sound.BossTank_Hover, transform);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (LasersActive && (transform.position - player.position).magnitude < 5f)
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, LaserStrikeDamage);
        }
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();

        if (!isFiring) { AimTurret(); }
        MaybeFire();
    }

    protected override void AttackPlayer()
    {
        // When the tank is close to the player (i.e., in its attack range), it will move away from the player
        if (!isFiring) { AimTurret(); }

        // Reset walk point if within target walkpoint or enough time has passed
        if ((walkPointSet && (transform.position - walkPoint).magnitude < 2f) || Time.time - LastWalkPointSetTime > 4f)
            walkPointSet = false;

        if (!walkPointSet && !isFiring)
        {
            willFire = false;
            if (Random.Range(0f, 1f) < RepositionChance && !willFire)
            {
                // Chance to reposition
                SearchForWalkPointAwayFromPlayer();
            }
            else if (Random.Range(0f, 1f) < LaserStrikeChance && !willFire && (transform.position - player.position).magnitude < LaserStrikeActivationRange)
            {
                // Chance to activate defensive laser strike if player is within range
                StartCoroutine(LaserStrike());
            }
            else if (Random.Range(0f, 1f) < DroneSpawnChance && !willFire)
            {
                // Chance to spawn a drone
                StartCoroutine(SpawnDrone());
            }
            else
            {
                willFire = true;
                MaybeFire();
            }
        }
    }

    IEnumerator LaserStrike()
    {
        isFiring = true;

        TargetingParticles.Play();
        SoundManager.PlaySound(SoundManager.Sound.Generic_Alert, transform.position);
        yield return new WaitForSeconds(4f);
        TargetingParticles.Stop();

        LaserStrikeParticles.Play();
        SoundManager.PlaySound(SoundManager.Sound.BossTank_Lasers, transform.position);
        LasersActive = true;
        yield return new WaitForSeconds(4f);
        LaserStrikeParticles.Stop();
        LasersActive = false;

        isFiring = false;
    }

    IEnumerator SpawnDrone()
    {
        isFiring = true;

        // Open the hatch in NFrames
        int NFrames = 100;
        float N2 = (float)NFrames / 2f;
        for (int i = 0; i < NFrames; i++)
        {
            float t = (float)i / (NFrames - 1); // Calculate interpolation factor based on current frame index
            HatchTransform.localRotation = Quaternion.Slerp(HatchTransform.localRotation, Quaternion.Euler(0f, 0f, -90f), t);
            yield return null; // wait for next frame
        }
        HatchTransform.localRotation = Quaternion.Euler(0f, 0f, -90f);

        var Drone = Instantiate(
                        MeleeDronePrefab,
                        HatchTransform.position,
                        Quaternion.identity
                    );

        var Vector = 2f * HatchTransform.forward;
        var Origin = HatchTransform.position - Vector;

        // Close the hatch and move drone in arc away from the tank
        for (int i = 0; i < NFrames; i++)
        {
            float t = (float)i / (NFrames - 1);
            HatchTransform.localRotation = Quaternion.Slerp(HatchTransform.localRotation, Quaternion.identity, t);

            // Move the drone along arc
            if (i < NFrames/2)
            {
                Vector = Quaternion.AngleAxis(-180f/(NFrames/2f), HatchTransform.right) * Vector;
                Drone.transform.position = Origin + Vector;
            }
            else
            {
                var temp = Origin + Vector;
                Drone.transform.position = Vector3.Lerp(temp, new Vector3(temp.x, 0.5f, temp.z), (i - N2) / N2);
            }

            yield return null; // wait for next frame
        }
        HatchTransform.localRotation = Quaternion.identity;

        isFiring = false;
    }

    void SearchForWalkPointAwayFromPlayer()
    {
        // Search for walkpoint away from the player if there is not one already set.
        int i = 0;
        while (!walkPointSet && i < 100)
        {
            Vector3 FromPlayer = player.position - transform.position; // vector from player to tank
            FromPlayer = new Vector3(FromPlayer.x, 0f, FromPlayer.z).normalized;

            // Similar to SearchWalkPoint but walkPoint is now along vector away from the player
            walkPoint = new Vector3(player.position.x + FromPlayer.x * Random.Range(-attackRange, attackRange), 
                                    player.position.y, 
                                    player.position.z + FromPlayer.z * Random.Range(-attackRange, attackRange));

            if (Physics.Raycast(walkPoint, -transform.up, 2f, Walkable))
            {
                walkPointSet = true;
                LastWalkPointSetTime = Time.time;
                agent.SetDestination(walkPoint);
            }

            i += 1;
        }

        if (i == 100)
            Debug.LogWarning("Failed to find a valid movement location in "+ i.ToString() + " tries!");
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
        float barrelAngle = Vector3.SignedAngle(TurretTransform.rotation * Vector3.forward, toPlayer, TurretTransform.rotation * Vector3.right) - 20f;
        // Limit bottom angle of barrel (note down for this model's barrel is positive, hence the sign flips!)
        if (-barrelAngle < MinBarrelAngle) {
            barrelAngle = -MinBarrelAngle;
        }
        CurrentBarrelAngles = Quaternion.Slerp(BarrelTransform.localRotation, Quaternion.Euler(barrelAngle, 0f, 0f), TurnRate * Time.deltaTime);
        BarrelTransform.localRotation = CurrentBarrelAngles;
    }

    void MaybeFire()
    {
        Vector3 playerPosn = player.position;
        Vector3 toPlayerFlat = new Vector3(playerPosn.x, TurretTransform.position.y, playerPosn.z) - TurretTransform.position; // Flat vector from turret to player

        // Determine the left/right angle to the player from current turret orientation
        float turretAngle = Mathf.Abs(Vector3.SignedAngle(TurretTransform.forward, toPlayerFlat, Vector3.up));

        if (turretAngle < BarrelShootingAngleRange && Time.time - TankMissileLastFireTime > TankMissileFireDelay && !isFiring)
        {
            StartCoroutine(FireSequence());
        }
    }

    private IEnumerator FireSequence()
    {
        willFire = false;
        isFiring = true;
        stunned = true; // tank cannot move during firing sequence

        // Show the attack indicator
        if (AttackIndicator != null)
        {
            AttackIndicator.SetActive(true);
        }

        // Wait for the specified delay time
        yield return new WaitForSeconds(AttackIndicatorDisplayTime);

        // Show the attack indicator
        if (AttackIndicator != null)
        {
            AttackIndicator.SetActive(false);
        }

        // Create missile instance
        var Missile = Instantiate(
            TankMissilePrefab,
            BarrelTransform.position + BarrelTransform.forward * TankMissileLaunchOffset,
            BarrelTransform.rotation);

        // Set missile velocity
        var MissileRB = Missile.GetComponent<Rigidbody>();
        Vector3 horizontalVelocity = new Vector3(BarrelTransform.forward.x, 0f, BarrelTransform.forward.z).normalized * TankMissileSpeed;
        MissileRB.velocity = horizontalVelocity;

        // Set missile params
        var MissileComp = Missile.GetComponent<TankMissile>();
        MissileComp.Damage = AttackDamage;
        MissileComp.MaxLifeSpan = TankMissileMaxLifeSpan;
        MissileComp.HomingStrength = TankMissileHomingStrength;

        TankMissileLastFireTime = Time.time;

        stunned = false;
        isFiring = false;

        // Add a "kick back" force at the end of the tank's barrel
        TankRigidbody.AddForceAtPosition(
            -BarrelTransform.forward * TankMissileSpeed * MissileRB.mass, // force
            BarrelTransform.position - 2 * BarrelTransform.forward, // apply force to end of barrel
            ForceMode.Impulse); // we want an instantaneous "pulse" to be applied
        TankMissileLastFireTime = Time.time;
        
        // Play firing soundFX
        SoundManager.PlaySound(SoundManager.Sound.Generic_FireMissile, transform.position, 0.5f);

        // Wait for the remainder of the fire delay
        yield return new WaitForSeconds(TankMissileFireDelay - AttackIndicatorDisplayTime);
        
    }

    void OnTriggerEnter(Collider col)
    {
        // Player takes damage if the tank hits them
        if (col.gameObject.tag == "Player")
        {
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, TankImpactDamage);
        }
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
