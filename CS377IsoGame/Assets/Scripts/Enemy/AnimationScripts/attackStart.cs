using UnityEngine;
using UnityEngine.AI;

public class ScreamBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the NavMeshAgent component from the enemy
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {

            // Set the speed to 0 to stop the enemy from moving
            agent.speed = 0;
        }
    }

    
}

