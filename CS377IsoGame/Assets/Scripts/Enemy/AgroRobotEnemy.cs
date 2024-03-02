using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroRobotEnemy : EnemyAI
{ 
    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Animator animator;
    public float dashSpeed = 30f; // Speed of the dash
    public float overshootDistance = 5f; // How far past the player the dash should go
    public float originalSpeed; // used for storing the speed
    public GameObject SlashAttackIndicator;
    public GameObject ThrustAttackIndicator;

    protected override void Awake()
    {
        base.Awake();

        // Initialize health and attack damage for this specific enemy
        Health = 100f;
        AttackDamage = 10f;
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
            animator.SetTrigger("StartAttack");

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

    public IEnumerator DashTowardsTarget(Vector3 target)
    {
        float dashStartTime = Time.time;
        float dashMaxDuration = 2.0f; // Maximum duration to prevent infinite dashing

        SoundManager.PlaySound(SoundManager.Sound.Generic_Slash_3, transform.position);

        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                new Vector3(target.x, 0f, target.z)) > 0.1f)
        {
            // Break out of the loop if the dash duration is exceeded
            if (Time.time - dashStartTime > dashMaxDuration)
            {
                Debug.LogWarning("Dash timeout reached");
                break;
            }

            Vector3 moveDirection = (new Vector3(target.x, transform.position.y, target.z) - transform.position).normalized;
            transform.position += moveDirection * dashSpeed * Time.deltaTime;

            yield return null; // Wait a frame and continue
        }

        // Play Sounds to match with animation
        yield return new WaitForSeconds(0.4f);
        SoundManager.PlaySound(SoundManager.Sound.Generic_Slash_2, transform.position);
        yield return new WaitForSeconds(0.2f);
        SoundManager.PlaySound(SoundManager.Sound.Generic_Explosion, transform.position);
    }


}
