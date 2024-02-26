using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretController : EnemyAI
{
    [SerializeField] Transform TurretTransform;
    [SerializeField] Transform BarrelTransform;
    [SerializeField] LineRenderer TargetingRenderer;
    [SerializeField] GameObject PowerDraw;
    [SerializeField] GameObject PowerBeam;

    public float TurnRate;
    public float BarrelShootingAngleRange; // Turret shoots iff barrel is within X deg off vector to character posn
    public float SpinUpRate; // Rate at which turret barrel rotation speeds up
    public float MaxSpinRate; // Max spin rate in degrees/update
    public float MinFireSpinPercent; // Min spin rate required to start firing as a proportion of MaxSpinRate [0,1] 
    public float MaxFireTime; // Max time before laser turns off
    public float CooldownTime;

    private float CurrentSpinRate;
    private float CurrentFireTime;
    private float CurrentCooldownTime;
    private bool InCooldown;

    private LayerMask PlayerLayerMask;
    private LayerMask WalkableLayerMask; // used to determine collisions with environment
    private StatsTemplate Stats;


    // For soundFX
    private enum State {
        spinningUp,
        spinning,
        spinningDown,
    }
    private State CurrentState;
    private GameObject TurningSoundFXContainer;
    private GameObject SpinningSoundFXContainer;
    private GameObject ChargingSoundFXContainer;
    private GameObject FiringSoundFXContainer;

    protected override void Awake()
    {
        base.Awake();

        if (TargetingRenderer != null)
        {
            TargetingRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("Missing Targeting Renderer!");
        }

        PlayerLayerMask = LayerMask.GetMask("Player");
        WalkableLayerMask = LayerMask.GetMask("Walkable");
        
        Health = 20f;
        healthBar.maxValue = Health;
        AttackDamage = 0.5f;
    }

    void Start()
    {
        // Initialize soundFX loop containers
        TurningSoundFXContainer = SoundManager.PlaySoundLoop(SoundManager.Sound.LaserTurret_Turning, transform);
        TurningSoundFXContainer.SetActive(false);

        SpinningSoundFXContainer = SoundManager.PlaySoundLoop(SoundManager.Sound.LaserTurret_Spinning, transform);
        SpinningSoundFXContainer.SetActive(false);
    }

    protected override void Patroling()
    {
        WindDown();
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        AimTurret();
        WindDown();
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
        float targetTurretAngle = Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(0f, targetTurretAngle, 0f);

        // Smooth rotation from current angle to desired angle
        Quaternion newRotation = Quaternion.RotateTowards(TurretTransform.rotation, targetRotation, TurnRate * Time.deltaTime);

        TurretTransform.rotation = newRotation;

        float currentTurretAngle = TurretTransform.rotation.eulerAngles.y;
        float deltaAngle = Mathf.Abs(Mathf.Abs(targetTurretAngle - currentTurretAngle + 180f) - 180f); // The double Abs are a hack to make sure deltaAngle >= 0
        if (deltaAngle > 5f)
        {
            TurningSoundFXContainer.SetActive(true);
        } 
        else 
        {
            TurningSoundFXContainer.SetActive(false);
        }
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

            PowerBeam.SetActive(true);
            TargetingRenderer.enabled = false; // Turn off targeting while firing

            // Check if laser hits player using a raycast
            // Note: For some reason, we need to set origin to transform.position since BarrelTransform.position causes laser to miss.
            if (Physics.Raycast(transform.position, BarrelTransform.forward, attackRange, PlayerLayerMask))
            {
                float distanceToPlayer = new Vector2(transform.position.x - player.position.x, transform.position.z - player.position.z).magnitude;

                // Do another ray cast to determine if the laser is blocked by something else along the path to the player
                if (!Physics.Raycast(transform.position, BarrelTransform.forward, distanceToPlayer, WalkableLayerMask))
                {
                    // Call the player's take damage event, deal damage per physics tick if it hits player
                    EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, AttackDamage);
                }
            }
        }
        else
        {
            PowerBeam.SetActive(false);
        }
    }

    void WindUp()
    {
        PowerDraw.SetActive(true);

        CurrentSpinRate = Mathf.Lerp(CurrentSpinRate, MaxSpinRate, SpinUpRate / 100f);
        BarrelTransform.Rotate(0f, 0f, CurrentSpinRate, Space.Self);

        UpdateTargetingRenderer(2f*CurrentSpinRate/MaxSpinRate);

        // Sound FX
        if (CurrentSpinRate/MaxSpinRate > MinFireSpinPercent && CurrentState != State.spinning)
        {
            if (ChargingSoundFXContainer != null)
                ChargingSoundFXContainer.SetActive(false);
            
            SpinningSoundFXContainer.SetActive(true);
            
            FiringSoundFXContainer = SoundManager.PlaySoundLoop(SoundManager.Sound.LaserTurret_Firing, transform);
            
            CurrentState = State.spinning;
        }
        else if (CurrentState != State.spinningUp && CurrentState != State.spinning)
        {
            SpinningSoundFXContainer.SetActive(false);
            
            if (FiringSoundFXContainer != null)
                FiringSoundFXContainer.SetActive(false);
            
            SoundManager.PlaySound(SoundManager.Sound.LaserTurret_SpinUp, transform.position);
            ChargingSoundFXContainer = SoundManager.PlaySoundLoop(SoundManager.Sound.LaserTurret_Charging, transform);
            
            CurrentState = State.spinningUp;
        }
    }

    void WindDown()
    {
        PowerDraw.SetActive(false);
        PowerBeam.SetActive(false);

        CurrentSpinRate = Mathf.Lerp(CurrentSpinRate, 0f, SpinUpRate / 100f);
        BarrelTransform.Rotate(0f, 0f, CurrentSpinRate, Space.Self);

        UpdateTargetingRenderer(2f*CurrentSpinRate/MaxSpinRate);

        // If not actively firing, cooldown without entering full cooldown
        if (CurrentFireTime > 0f)
        {
            CurrentFireTime -= Time.deltaTime;
        }

        // SoundFX
        if (CurrentState != State.spinningDown)
        {
            SpinningSoundFXContainer.SetActive(false);
            
            if (ChargingSoundFXContainer != null)
                ChargingSoundFXContainer.SetActive(false);
            
            if (FiringSoundFXContainer != null)
                FiringSoundFXContainer.SetActive(false);
            
            SoundManager.PlaySound(SoundManager.Sound.LaserTurret_SpinDown, transform.position);
            
            CurrentState = State.spinningDown;
        }
    }

    void UpdateTargetingRenderer(float alpha)
    {
        RaycastHit hit;
        Vector3 start = BarrelTransform.position;
        Vector3 direction = BarrelTransform.forward;

        TargetingRenderer.enabled = true;
        if (Physics.Raycast(start, direction, out hit, attackRange, WalkableLayerMask))
        {
            TargetingRenderer.SetPosition(0, start);
            TargetingRenderer.SetPosition(1, hit.point);
        }
        else
        {
            TargetingRenderer.SetPosition(0, start);
            TargetingRenderer.SetPosition(1, start + direction * attackRange);
        }

        if (TargetingRenderer == null || TargetingRenderer.material == null)
        {
            Debug.LogError("Missing tageting line renderer or material!");
        }
        Color color = TargetingRenderer.material.color;
        color.a = Mathf.Clamp01(alpha);
        TargetingRenderer.material.color = color;
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
