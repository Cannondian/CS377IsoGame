using UnityEngine;

public class BaseEnemyAI : EnemyAI
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
        Health = 100f;
        AttackDamage = 10f;
    }

    protected override void Patroling()
    {
        base.Patroling();
        ChangeColor(Color.blue);
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        ChangeColor(Color.yellow);
    }

    protected override void AttackPlayer()
    {
        ChangeColor(Color.red);

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
}
