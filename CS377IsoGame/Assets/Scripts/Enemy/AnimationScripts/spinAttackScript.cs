using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinAttackScript : StateMachineBehaviour
{
    private Transform playerTransform;
    private AgroRobotEnemy enemyAI;
    private GameObject attackIndicator;

    private float SpinAttackDelay = 0.0f; // nevermind...

    // OnStateEnter is called right before any state animations start playing
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyAI = animator.GetComponent<AgroRobotEnemy>();
        animator.SetFloat("SinAttackDelay", SpinAttackDelay);
        enemyAI.StartCoroutine(AttackPlayer(animator, stateInfo, layerIndex));
    }

    IEnumerator AttackPlayer(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        attackIndicator = enemyAI.SlashAttackIndicator;

        // Give the player a bit of breathing room
        yield return new WaitForSeconds(SpinAttackDelay);

        // Face the player
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
