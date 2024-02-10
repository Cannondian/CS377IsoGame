using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



//reminder that the name for this resource will be changed to "Core Charge"

    public class CoreChargeManager : Singleton<CoreChargeManager>
    {

        public int coreChargeState { get; set; }
        private int coreChargeDrain;


        public int previousUpdateSentAt;
        public bool enhancedAttackReady;

        private bool atBaseValue;
        private bool isDraining;
        private bool recentlyAttacked;
        private float timerStarted;
        private float drainThresholdTime = 8;
        private float countdownToDrain;

        private UnityAction<EventTypes.Event5Param> UpdateCoreChargeListener;
        private UnityAction<bool> CoreChargeAttackListener;

        public Slider coreChargeSlider;

        // Start is called before the first frame update
        void Start()
        {
            coreChargeState = 0;
            coreChargeDrain = 2;
            previousUpdateSentAt = 0;
            atBaseValue = true;
            countdownToDrain = drainThresholdTime;
            UpdateCoreChargeParticles();
        }

        // Update is called once per frame
        void Update()
        {
            coreChargeSlider.value = coreChargeState;
            if (!atBaseValue)
            {
                if (countdownToDrain == drainThresholdTime)
                {
                    timerStarted = Time.time;
                    countdownToDrain = drainThresholdTime - 0.1f;
                    recentlyAttacked = false;

                }
                else if (isDraining)
                {
                    if (coreChargeState <= 0)
                    {
                        coreChargeState = 0;
                        isDraining = false;
                        atBaseValue = true;
                        countdownToDrain = drainThresholdTime;
                        coreChargeDrain = 2;

                        UpdateCoreChargeParticles();


                    }
                    else if (recentlyAttacked)
                    {
                        isDraining = false;
                        countdownToDrain = drainThresholdTime;
                        coreChargeDrain = 2;
                    }
                    else if (Time.time - timerStarted > 1)
                    {
                        timerStarted += 2;
                        coreChargeState -= coreChargeDrain;
                        coreChargeDrain = 2 * coreChargeDrain;

                        UpdateCoreChargeParticles();



                    }
                }
                else if (countdownToDrain <= 0)
                {
                    timerStarted = Time.time;
                    isDraining = true;




                }
                else if (Time.time - timerStarted > 1)
                {
                    countdownToDrain--;
                    timerStarted += 2;
                }

            }


        }

        private void UpdateCoreCharge(EventTypes.Event5Param context)
        {
            recentlyAttacked = true;
            
            if (coreChargeState >= 45)
            {
                enhancedAttackReady = true;
            }
            else
            {



                switch (context.attackNumber)
                {
                    case 0:
                        coreChargeState += 2;
                        break;
                    case 1:
                        coreChargeState += 3;
                        break;
                    case 2:
                        coreChargeState += 4;
                        break;
                    case 3:
                        coreChargeState += 7;
                        break;

                }
            }

            UpdateCoreChargeParticles();


            


            if (atBaseValue)
            {
                atBaseValue = false;
            }


        }

        public void CoreChargeAttack(bool isTriggered)
        {
            if (isTriggered)
            {
                
                enhancedAttackReady = !isTriggered;

                coreChargeState = 0;
                isDraining = false;
                atBaseValue = true;
                countdownToDrain = drainThresholdTime;
                coreChargeDrain = 2;


                UpdateCoreChargeParticles();

            }
        }

        private void UpdateCoreChargeParticles()
        {
            if (Mathf.Abs(coreChargeState - previousUpdateSentAt) >= 10 || coreChargeState == 0)
            {
                previousUpdateSentAt = coreChargeState;
                EventBus.TriggerEvent(EventTypes.Events.ON_UPDATE_CORE_CHARGE_PARTICLES,
                    new EventTypes.Event8Param(coreChargeState, false, FXList.FXlist.ElectricityFX2));
            }
        }


        private void OnEnable()
        {

            UpdateCoreChargeListener += UpdateCoreCharge;
            EventBus.StartListening(EventTypes.Events.ON_LIFE_CURRENT_GAIN, UpdateCoreChargeListener);
            CoreChargeAttackListener += CoreChargeAttack;
            EventBus.StartListening(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, CoreChargeAttackListener);



        }

        private void OnDisable()
        {

            EventBus.StopListening(EventTypes.Events.ON_LIFE_CURRENT_GAIN, UpdateCoreChargeListener);
            EventBus.StopListening(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, CoreChargeAttackListener);


        }

    }
