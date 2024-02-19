using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlowEnemy : EnemyAI
{
    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Animator animator;
    public float originalSpeed; // used for storing the speed
    public GameObject attackIndicator;

    protected override void Awake()
    {
        base.Awake();

        // Initialize health and attack damage for this specific enemy
        Health = 100f;
        AttackDamage = 20f;
        originalSpeed = agent.speed;
    }

    protected override void Patroling()
    {
        base.Patroling();

    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();

    }

    protected override void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            // Trigger attack pattern
            animator.SetTrigger("AttackTrigger");


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;

    }
}
