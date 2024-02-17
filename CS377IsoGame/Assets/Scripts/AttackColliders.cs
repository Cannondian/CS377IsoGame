using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackColliders : Singleton<AttackColliders>
{
    [SerializeField] private GameObject shockStaff;
    [SerializeField] private GameObject ultraLeg;
    [SerializeField] private CapsuleCollider staffOriginalCollider;
    [SerializeField] private BoxCollider legOriginalCollider;
    [SerializeField] private Animator staffAnimator;
    [SerializeField] private Animator legAnimator;
    


    private bool attacked;
    private int trigger1;
    private int trigger2;
    private int trigger5;
    private int speed;
    private int legExtending;
    private float legExtendingDuration;
    
    public void TriggerColliderAnimation(int attackNumber, float attackSpeed)
    {
        
        if (attackNumber == 1)
        {
            staffAnimator.ResetTrigger(trigger5);
            staffAnimator.ResetTrigger(trigger2);
            staffAnimator.SetTrigger(trigger1);
            staffAnimator.SetFloat(speed, attackSpeed);
            
        }

        if (attackNumber == 2)
        {
            staffAnimator.ResetTrigger(trigger5);
            staffAnimator.ResetTrigger(trigger1);
            staffAnimator.SetTrigger(trigger2);
            staffAnimator.SetFloat(speed, attackSpeed);
            
        }

        if (attackNumber == 5)
        {
            
            
            legAnimator.SetFloat(speed, attackSpeed);
            legAnimator.SetBool(legExtending, true);
            StartCoroutine(DisableLegAnimator(attackSpeed));
            Debug.Log("leg collider ");
            
        }

        
    }

    public IEnumerator DisableLegAnimator(float attackSpeed)
    {
        yield return new WaitForSeconds(
            1.25f/ attackSpeed);
        legAnimator.SetBool(legExtending, false);


    }
    // Start is called before the first frame update
    void Start()
    {
        trigger1 = Animator.StringToHash("Attack1");
        trigger2 = Animator.StringToHash("Attack2");
        trigger5 = Animator.StringToHash("Attack5");
        speed = Animator.StringToHash("AttackSpeed");
        legExtending = Animator.StringToHash("Ongoing");
        

    }

    

    
    
}
