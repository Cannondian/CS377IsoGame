using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDroneController : EnemyAI
{
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private float attackIndicatorDisplayTime = 1f;

    // Attack timing variables
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // Attack pattern variables
    public float dashSpeed;
    public float overshootDistance;
    private Vector3 attackTarget;
    public float pauseAfterAttack = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        attackIndicator.SetActive(false);

        // Initialize health and attack damage for this specific enemy
        Health = 30f;
        AttackDamage = 10f;
    }

    protected override void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            // Calculate the overshoot target, maintaining the same Y-axis (height)
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 attackTarget = new Vector3(
                player.position.x + directionToPlayer.x * overshootDistance,
                transform.position.y, // Keep original Y-axis
                player.position.z + directionToPlayer.z * overshootDistance
            );

            StartCoroutine(DashTowardsTarget(attackTarget));

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private IEnumerator DashTowardsTarget(Vector3 target)
    {
        stunned = true;

        // handle strange cases
        float dashStartTime = Time.time;
        float dashMaxDuration = 2.0f;

        // Show the attack indicator
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(true);
        }

        // Wait for the specified delay time
        yield return new WaitForSeconds(attackIndicatorDisplayTime);

        // Show the attack indicator
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(false);
        }

        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                new Vector3(target.x, 0f, target.z)) > 0.1f)
        {
            // Break out of the loop if the dash duration is exceeded
            if (Time.time - dashStartTime > dashMaxDuration)
            {
                Debug.Log("Dash timeout reached");
                break;
            }

            Vector3 moveDirection = (new Vector3(target.x, transform.position.y, target.z) - transform.position).normalized;
            transform.position += moveDirection * dashSpeed * Time.deltaTime;

            yield return null;
        }

        // Pause briefly at the target point
        yield return new WaitForSeconds(pauseAfterAttack);

        stunned = false;
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
