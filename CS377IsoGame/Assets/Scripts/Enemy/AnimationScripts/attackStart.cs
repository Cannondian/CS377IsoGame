using UnityEngine;
using UnityEngine.AI;

public class ScreamBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private AgroRobotEnemy enemyAI;
    private GameObject thrustAttackIndicator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyAI = animator.GetComponent<AgroRobotEnemy>();
        thrustAttackIndicator = enemyAI.ThrustAttackIndicator;
        // Get the NavMeshAgent component from the enemy
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {

            // Set the speed to 0 to stop the enemy from moving
            agent.speed = 0;
            thrustAttackIndicator.GetComponent<AgroMeleeSwordCollider>().ActivateWithDelay(0.5f);

            // PlaysoundFX for yell
            SoundManager.PlaySound(SoundManager.Sound.Generic_Yell, agent.transform.position);
        }
    }

    
}

