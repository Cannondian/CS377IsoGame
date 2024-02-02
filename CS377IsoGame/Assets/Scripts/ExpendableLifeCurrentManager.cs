using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



    public class ExpendableLifeCurrentManager : Singleton<ExpendableLifeCurrentManager>
    {

        public int ELCState { get; set; }
        private int ELCdrain;
        private bool enhancedAttackUsed;
        private bool atBaseValue;
        private bool isDraining;
        private bool recentlyAttacked;
        private float timerStarted;
        private float drainThresholdTime = 8;
        private float countdownToDrain;
        private UnityAction<EventTypes.Event5Param> updateELCListener;
        private UnityAction<bool> ELCAttackListener;

        // Start is called before the first frame update
        void Start()
        {
            ELCState = 0;
            ELCdrain = 2;
            atBaseValue = true;
            countdownToDrain = drainThresholdTime;
        }

        // Update is called once per frame
        void Update()
        {
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
                    if (ELCState <= 0)
                    {
                        ELCState = 0;
                        isDraining = false;
                        atBaseValue = true;
                        countdownToDrain = drainThresholdTime;
                        ELCdrain = 2;
                    }
                    else if (recentlyAttacked)
                    {
                        isDraining = false;
                        countdownToDrain = drainThresholdTime;
                        ELCdrain = 2;
                    }
                    else if (Time.time - timerStarted > 1)
                    {
                        timerStarted += 2;
                        ELCState -= ELCdrain;
                        ELCdrain = 2 * ELCdrain;

                    }
                }
                else if (countdownToDrain <= 0)
                {
                    timerStarted = Time.time;
                    isDraining = true;
                    EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK_READY, false);
                }
                else if (Time.time - timerStarted > 1)
                {
                    countdownToDrain--;
                    timerStarted += 2;
                }

            }

            
        }

        private void UpdateELC(EventTypes.Event5Param context)
        {
            recentlyAttacked = true;
            switch (context.attackNumber)
            {
                case 0:
                    ELCState += 2;
                    break;
                case 1:
                    ELCState += 3;
                    break;
                case 2:
                    ELCState += 4;
                    break;
                case 3:
                    ELCState += 7;
                    break;
                
            }
            
            if (atBaseValue)
            {
                atBaseValue = false;
            }

            if (ELCState > 45)
            {
                EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK_READY, true);
            }
        }

        public void ELCAttack(bool isTriggered)
        {
            if (isTriggered)
            {
                enhancedAttackUsed = isTriggered;
                ELCState = 0;
                isDraining = false;
                atBaseValue = true;
                countdownToDrain = drainThresholdTime;
                ELCdrain = 2;
                EventBus.TriggerEvent(EventTypes.Events.ON_JISA_ENHANCED_ATTACK_READY, false);
            }
        }

        private void OnEnable()
        {
            updateELCListener += UpdateELC;
            EventBus.StartListening(EventTypes.Events.ON_LIFE_CURRENT_GAIN, updateELCListener);
            ELCAttackListener += ELCAttack;
            EventBus.StartListening(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, ELCAttackListener);
        }

        private void OnDisable()
        {
            EventBus.StopListening(EventTypes.Events.ON_LIFE_CURRENT_GAIN, updateELCListener);
            EventBus.StopListening(EventTypes.Events.ON_JISA_ENHANCED_ATTACK, ELCAttackListener);

        }

    }
