using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackColliders : Singleton<AttackColliders>
{
    [SerializeField] private GameObject shockStaff;
    [SerializeField] private GameObject ultraLeg;
    [SerializeField] private CapsuleCollider staffOriginalCollider;
    [SerializeField] private CapsuleCollider legOriginalCollider;
    [SerializeField] private Animator staffAnimator;
    


    private int trigger1;
    private int trigger2;
    private int trigger5;
    private int speed;
    
    public void TriggerColliderAnimation(int attackNumber, float attackSpeed)
    {
        
        if (attackNumber == 1)
        {
            staffAnimator.ResetTrigger(trigger5);
            staffAnimator.ResetTrigger(trigger2);
            staffAnimator.SetTrigger(trigger1);
            staffAnimator.SetFloat(speed, attackSpeed);
            staffOriginalCollider.enabled = true;
        }

        if (attackNumber == 2)
        {
            staffAnimator.ResetTrigger(trigger5);
            staffAnimator.ResetTrigger(trigger1);
            staffAnimator.SetTrigger(trigger2);
            staffAnimator.SetFloat(speed, attackSpeed);
            staffOriginalCollider.enabled = true;
        }

        if (attackNumber == 5)
        {
            staffAnimator.ResetTrigger(trigger1);
            staffAnimator.ResetTrigger(trigger2);
            staffAnimator.SetTrigger(trigger5);
            staffAnimator.SetFloat(speed, attackSpeed);
            legOriginalCollider.enabled = true;
        }
    }

    public void ResetColliders()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        trigger1 = Animator.StringToHash("Attack1");
        trigger2 = Animator.StringToHash("Attack2");
        trigger5 = Animator.StringToHash("Attack5");
        speed = Animator.StringToHash("AttackSpeed");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        legOriginalCollider.enabled = false;
        staffOriginalCollider.enabled = false;
        
    }

    
}
