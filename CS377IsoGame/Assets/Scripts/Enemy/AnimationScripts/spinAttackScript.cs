using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinAttackScript : StateMachineBehaviour
{
    private Transform playerTransform;

    // OnStateEnter is called right before any state animations start playing
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Immediately face the player
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - animator.transform.position).normalized;
            // Ensure rotation only happens around the y-axis
            directionToPlayer.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            animator.transform.rotation = lookRotation;
        }

        // Here you can also initialize the preparation for the circular attack wave
        // For example, triggering an animation flag or preparing a visual effect
    }
}
