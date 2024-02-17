using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombatCollisions : MonoBehaviour
{
    
  
        //one idea to make this better would be to turn on their colliders at the end of the attack animation
        [SerializeField] private RPGCharacterController controller;
        [SerializeField] private GameObject shockStaff;
        [SerializeField] private GameObject ultraLeg;
        [SerializeField] private CapsuleCollider staffOriginalCollider;
        [SerializeField] private BoxCollider legOriginalCollider;
        [SerializeField] private Animator staffAnimator;
        [SerializeField] private Animator legAnimator;

        private Rigidbody staffBody;
        

        private bool attacked;
        private int trigger1;
        private int trigger2;
        private int trigger5;
        private int speed;
        private int legExtending;
        private float legExtendingDuration;
        
        private int attackNumber;
        private bool isNewAttack;
        private float attackTimer;
        private bool attackDone;
        
        
        void Start()
        {
            
            
            trigger1 = Animator.StringToHash("Attack1");
            trigger2 = Animator.StringToHash("Attack2");
            trigger5 = Animator.StringToHash("Attack5");
            speed = Animator.StringToHash("AttackSpeed");
            legExtending = Animator.StringToHash("Ongoing");
            staffBody = shockStaff.GetComponent<Rigidbody>();



        }

        private void Update()
        {
            
            
        }

        public void TriggerColliderAnimation(int attackNumber, float attackSpeed)
        {
            this.attackNumber = attackNumber;
            isNewAttack = true;
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
        
        private void OnTriggerStay(Collider other)
        {
                if (isNewAttack)
                {
                    if (attackNumber == 3)
                    {
                        Debug.Log("collided");
                        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                            new EventTypes.HitFXParam(transform.position, attackNumber,
                                gameObject));

                        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                            CharacterEnergy.Instance.energyFromEnhancedBasic);

                        EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, true);

                        DamageEnemy(other, 80f);
                        EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                            new EventTypes.FloatingDamageParam(other.gameObject, 80));
                    }
                    else
                    {
                        EventBus.TriggerEvent(EventTypes.Events.ON_LIFE_CURRENT_GAIN,
                            new EventTypes.Event5Param(attackNumber, 0));

                        EventBus.TriggerEvent(EventTypes.Events.ON_BASIC_ATTACK_HIT,
                            new EventTypes.HitFXParam(GetHitPoint(other),
                                attackNumber,
                                gameObject));
                        EventBus.TriggerEvent(EventTypes.Events.ON_ENERGY_GAIN,
                            CharacterEnergy.Instance.energyFromEnhancedBasic);
                            EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, 
                                new EventTypes.FloatingDamageParam(other.gameObject, 6));
                            Debug.Log("collided");
                            DamageEnemy(other, 6f);
                        
                    }

                    isNewAttack = false;
                }

            }

        Vector3 GetHitPoint(Collider other)
        {
            Vector3 dir = (other.ClosestPointOnBounds(transform.position) - transform.position).normalized;
            RaycastHit hit;
            Vector3 position = Vector3.zero;
            if (Physics.Raycast(transform.position, dir, out hit, 10, 1 << 15, QueryTriggerInteraction.Collide))
            {
                 position = hit.point;
            }

            return position;
        }

         
        void DamageEnemy(Collider other, float damage)
        {
            var enemyHealth = other.gameObject.GetComponent<Health>();
            if (enemyHealth.amIEnemy)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
}

