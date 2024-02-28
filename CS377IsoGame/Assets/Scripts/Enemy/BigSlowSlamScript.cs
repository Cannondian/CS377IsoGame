using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigSlowSlamScript : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private BigSlowEnemy enemyAI;
    private GameObject attackIndicator;

    private float StartTime;
    private bool PlayedExplosion;

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

        StartTime = Time.time;
        PlayedExplosion = false;
    }

    public void OnStateUpdate()
    {
        if (!PlayedExplosion && Time.time - StartTime > 2f) // 2f approximates time to explosion in animation
        {
            PlayedExplosion = true;
            SoundManager.PlaySound(SoundManager.Sound.Generic_Explosion, agent.transform.position);
        }
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
