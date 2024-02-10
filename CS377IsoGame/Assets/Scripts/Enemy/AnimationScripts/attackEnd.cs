using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class attackEnd : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private AgroRobotEnemy enemyAI;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the enemy AI script
        enemyAI = animator.GetComponent<AgroRobotEnemy>();
        // Get the NavMeshAgent component from the enemy
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the speed to the original value
        if (agent != null)
        {
            agent.speed = enemyAI.originalSpeed;
            animator.ResetTrigger("StartAttack");
        }
    }
}
