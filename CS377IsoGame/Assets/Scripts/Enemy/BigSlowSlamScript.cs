using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigSlowSlamScript : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private BigSlowEnemy enemyAI;
    private GameObject attackIndicator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the enemy AI script
        enemyAI = animator.GetComponent<BigSlowEnemy>();
        attackIndicator = enemyAI.attackIndicator;
        // Get the NavMeshAgent component from the enemy
        agent = animator.gameObject.GetComponent<NavMeshAgent>();

        agent.speed = 0;

        attackIndicator.SetActive(true);
        attackIndicator.GetComponent<AttackIndicator>().ActivateIndicator();


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the speed to the original value
        if (agent != null)
        {
            agent.speed = enemyAI.originalSpeed;
            animator.ResetTrigger("AttackTrigger");
            attackIndicator.SetActive(false);

        }

    }

}
