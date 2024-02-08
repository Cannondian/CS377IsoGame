using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretController : EnemyAI
{
    [SerializeField] Transform TurretTransform;
    [SerializeField] Transform BarrelTransform;
    [SerializeField] LineRenderer LaserRenderer;

    public float TurnRate;
    public float BarrelShootingAngleRange; // Turret shoots iff barrel is within X deg off vector to character posn
    public float SpinUpTime; // Turret barrel rotation speeds up for this time before firing.
    public float MaxSpinRate; // Max spin rate in degrees/update
    public float MinFireSpinPercent; // Min spin rate required to start firing as a proportion of MaxSpinRate [0,1] 
    public float MaxFireTime; // Max time before laser turns off
    public float CooldownTime;


    private Quaternion CurrentTurretAngles;
    private float CurrentSpinRate;
    private float CurrentFireTime;
    private float CurrentCooldownTime;
    private bool InCooldown;

    private LayerMask playerLayerMask;

    protected override void Awake()
    {
        base.Awake();

        if (LaserRenderer != null)
        {
            LaserRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("Missing Laser Renderer!");
        }

        playerLayerMask = LayerMask.GetMask("Player");

        Health = 100f;
        AttackDamage = 0.5f;
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
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
        Vector3 toPlayer = playerPosn - TurretTransform.position; // vector from turret to player

        // Determine the left/right angle to the player
        float turretAngle = Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);

        // Smooth rotation from current angle to desired angle
        CurrentTurretAngles = Quaternion.Slerp(TurretTransform.rotation, Quaternion.Euler(0f, turretAngle, 0f), TurnRate * Time.deltaTime);
        TurretTransform.rotation = CurrentTurretAngles;
    }

    void MaybeFire()
    {
        Vector3 playerPosn = player.position;
        Vector3 toPlayerFlat = new Vector3(playerPosn.x, TurretTransform.position.y, playerPosn.z) - TurretTransform.position; // Flat vector from turret to player

        // Determine the left/right angle to the player from current turret orientation
        float turretAngle = Mathf.Abs(Vector3.SignedAngle(BarrelTransform.forward, toPlayerFlat, Vector3.up));

        if (!InCooldown)
        {
            // If turret reaches max fire time, it enters cooldown phase and cannot fire until reaching cooldown time
            if (CurrentFireTime >= MaxFireTime)
            {
                InCooldown = true;
            }

            // If barrel is within shooting angle, start/continue fire sequence
            if (turretAngle < BarrelShootingAngleRange)
            {
                FireSequence();
            }
            else
            {
                WindDown();
            }
        }
        else
        {
            // Turret is in cooldown
            WindDown();

            CurrentCooldownTime += Time.deltaTime;
            
            if (CurrentCooldownTime > CooldownTime)
            {
                InCooldown = false;
                CurrentCooldownTime = 0f;
                CurrentFireTime = 0f;
            }
        }

    }

    void FireSequence()
    {
        WindUp();

        if (CurrentSpinRate > MinFireSpinPercent * MaxSpinRate)
        {
            // Only add fire time if laser is actively on
            CurrentFireTime += Time.deltaTime;

            // Turn on and set laser renderer
            LaserRenderer.SetPosition(0, BarrelTransform.position);
            LaserRenderer.SetPosition(1, BarrelTransform.position + BarrelTransform.forward * attackRange);
            LaserRenderer.enabled = true;

            // Check if laser hits player using a raycast
            // Note: For some reason, we need to set origin to transform.position since BarrelTransform.position causes laser to miss.
            if (Physics.Raycast(transform.position, BarrelTransform.forward, attackRange, playerLayerMask))
            {
                // Call the player's take damage event, deal damage per physics tick if it hits player
                EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, AttackDamage);
            }
        }
        else
        {
            LaserRenderer.enabled = false;
        }
    }

    void WindUp()
    {
        CurrentSpinRate = Mathf.Lerp(CurrentSpinRate, MaxSpinRate, SpinUpTime / 100f);
        BarrelTransform.Rotate(0f, 0f, CurrentSpinRate, Space.Self);
    }

    void WindDown()
    {
        CurrentSpinRate = Mathf.Lerp(CurrentSpinRate, 0f, SpinUpTime / 100f);
        BarrelTransform.Rotate(0f, 0f, CurrentSpinRate, Space.Self);

        LaserRenderer.enabled = false;

        // If not actively firing, cooldown without entering full cooldown
        if (CurrentFireTime > 0f)
        {
            CurrentFireTime -= Time.deltaTime;
        }
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
