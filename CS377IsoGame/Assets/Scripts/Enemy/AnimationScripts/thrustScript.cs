using UnityEngine;
using UnityEngine.AI;

public class ThrustBehaviour : StateMachineBehaviour
{
    private Transform playerTransform;
    private Vector3 attackTarget;
    private AgroRobotEnemy enemyAI;
    private float dashSpeed;
    private float overshootDistance;
    private GameObject thrustAttackIndicator;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the enemy AI script and Weapon Indicator
        enemyAI = animator.GetComponent<AgroRobotEnemy>();
        thrustAttackIndicator = enemyAI.ThrustAttackIndicator;


        // Get the player transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Set the dash speed and overshoot distance from the enemyAI script
        dashSpeed = enemyAI.dashSpeed;
        overshootDistance = enemyAI.overshootDistance;

       

        // Calculate the overshoot target, keeping the y-axis the same
        Vector3 directionToPlayer = (playerTransform.position - animator.transform.position).normalized;
        attackTarget = playerTransform.position + directionToPlayer * overshootDistance;
        directionToPlayer.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        animator.transform.rotation = lookRotation;


        // Disable the NavMeshAgent for manual control
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // Begin the dash coroutine on the enemyAI script
        enemyAI.StartCoroutine(enemyAI.DashTowardsTarget(attackTarget));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Re-enable the NavMeshAgent
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = true;
        thrustAttackIndicator.SetActive(false);
    }
}
