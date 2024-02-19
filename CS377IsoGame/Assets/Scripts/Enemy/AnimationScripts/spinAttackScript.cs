using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinAttackScript : StateMachineBehaviour
{
    private Transform playerTransform;
    private AgroRobotEnemy enemyAI;
    private GameObject attackIndicator;

    // OnStateEnter is called right before any state animations start playing
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAI = animator.GetComponent<AgroRobotEnemy>();
        attackIndicator = enemyAI.SlashAttackIndicator;

        // Immediately face the player
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - animator.transform.position).normalized;
            // Ensure rotation only happens around the y-axis
            directionToPlayer.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            animator.transform.rotation = lookRotation;
            attackIndicator.GetComponent<AgroMeleeAttackCollider>().ActivateIndicator();
        }

        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackIndicator.SetActive(false);

    }
}
