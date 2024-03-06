using UnityEngine;

public class MeleeHumanEnemyAI : EnemyAI
{
    private Renderer enemyRenderer;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    protected override void Awake()
    {
        base.Awake();
        enemyRenderer = GetComponent<Renderer>();

        // Initialize health and attack damage for this specific enemy
        Health = 1000000f;
        AttackDamage = 0f;
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

        // Enemy stops when in attack range
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Implement specific attack logic for BaseEnemyAI

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    

    // Method to change the color of the enemy
    private void ChangeColor(Color color)
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = color;
        }
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }
}
