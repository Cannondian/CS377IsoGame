using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingGrenadierController : EnemyAI
{
    [SerializeField] Transform TurretTransform; // I.e., the "head" of the enemy where grenades are launched from
    [SerializeField] Transform TurretBottomTransform;

    [SerializeField] GameObject MissilePrefab;

    public float TurnRate;


    protected override void Awake()
    {
        base.Awake();

        Health = 100f;
        AttackDamage = 10f;
    }

    void AimTurret()
    {
        Vector3 playerPosn = player.position;
        Vector3 toPlayer = playerPosn - TurretTransform.position; // vector from turret to player

        // Determine the left/right angle to the player
        float turretAngle = Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);

        // Smooth rotation from current angle to desired angle
        // The -60f is to keep the green indicator facing the player
        TurretTransform.rotation = Quaternion.Slerp(TurretTransform.rotation, Quaternion.Euler(0f, turretAngle - 60f, 0f), TurnRate * Time.deltaTime);

        // If we want the bottom section to spin opposite the "head"
        TurretBottomTransform.rotation = Quaternion.Inverse(TurretTransform.rotation);
        
        // If we want the bottom section to remain stationary relative to the body
        // TurretBottomTransform.rotation = Quaternion.identity;
    }

    protected override void ChasePlayer()
    {
        AimTurret();
    }

    protected override void AttackPlayer()
    {
        AimTurret();
        // TODO: Fire
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
