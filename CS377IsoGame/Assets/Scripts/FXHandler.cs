using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


namespace RPGCharacterAnims.Actions
{
    
    public class FXHandler : Singleton<FXHandler>
    {
        #region Delegates

        private UnityAction<EventTypes.Event1Param> InitializeFXListener;
        private UnityAction<EventTypes.Event2Param> ContinousFXListener;
        private UnityAction<EventTypes.Event8Param> VariableFXListener;
        

        #endregion
        
        #region Fields

        public int coreChargeParticleLevel;

        #endregion

        #region FXIPrefabs

        public GameObject ArtilleryStrikePrefab;
        public GameObject ElectricityPrefab1;
        public GameObject ElectricityPrefab2;
        public GameObject ElectricityPrefab3;
        
        #endregion

        #region FXInstances

        public GameObject coreChargeParticles;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
            OnEnable();
            Debug.Log("we are many");
            
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            
            ActiveFX();
        }

        private void OnEnable()
        {
            InitializeFXListener += InitializeArtilleryStrikeFX;
            EventBus.StartListening(EventTypes.Events.ON_PARTICLE_FX_TRIGGER, InitializeFXListener);
            ContinousFXListener += InitializeElectricityFX;
            EventBus.StartListening(EventTypes.Events.ON_CONTINOUS_PACRTICLE_FX_TRIGGER,
                ContinousFXListener);
            VariableFXListener += UpdateCoreChargeFX;
            EventBus.StartListening(EventTypes.Events.ON_UPDATE_CORE_CHARGE_PARTICLES, VariableFXListener);
        }

        private void OnDisable()
        {
           EventBus.StopListeningAll<EventTypes.Events>();
        }
        // PH is for placeholder
        private void SetFlags(string color, Vector3 pos, FXList.FXlist effect, float duration)
        {

        }

        private void UpdateCoreChargeFX(EventTypes.Event8Param context)
        {
            var particlesToEdit = coreChargeParticles.GetComponent<ParticleSystem>().limitVelocityOverLifetime;
            var particlesToEdit2 = coreChargeParticles.GetComponent<ParticleSystem>().velocityOverLifetime;
            var particlesToEdit3 = coreChargeParticles.GetComponent<ParticleSystem>().main;
            if (context.coreCharge == 0)
            {
                particlesToEdit.dragMultiplier = 5;
                coreChargeParticleLevel = 0;
            }
            else if (context.coreCharge <= 10)
            {
                particlesToEdit.dragMultiplier = 1.4f;
                coreChargeParticleLevel = 1;
            }
            else if (context.coreCharge <= 20)
            {
                particlesToEdit.dragMultiplier = 1.2f;
                particlesToEdit2.orbitalXMultiplier = 3;
                particlesToEdit2.orbitalYMultiplier = 3;
                particlesToEdit2.orbitalZMultiplier = 3;
                particlesToEdit3.startLifetimeMultiplier = 1.2f;
                particlesToEdit3.simulationSpeed = 1.3f;
                coreChargeParticleLevel = 2;
            }
            else if (context.coreCharge <= 30)
            {
                particlesToEdit.dragMultiplier = 1;
                particlesToEdit2.orbitalXMultiplier = 5;
                particlesToEdit2.orbitalYMultiplier = 5;
                particlesToEdit2.orbitalZMultiplier = 5;
                particlesToEdit3.startLifetimeMultiplier = 1.4f;
                particlesToEdit3.simulationSpeed = 1.5f;
                coreChargeParticleLevel = 3;
                
            }
            else if (context.coreCharge <= 40)
            {
                particlesToEdit.dragMultiplier = 0.9f;
                particlesToEdit2.orbitalXMultiplier = 8;
                particlesToEdit2.orbitalYMultiplier = 8;
                particlesToEdit2.orbitalZMultiplier = 8;
                particlesToEdit3.startLifetimeMultiplier = 1.6f;
                particlesToEdit3.simulationSpeed = 1.8f;
                coreChargeParticleLevel = 4;
            }
            else
            {
                particlesToEdit.dragMultiplier = 0.8f;
                particlesToEdit2.orbitalXMultiplier = 10;
                particlesToEdit2.orbitalYMultiplier = 10;
                particlesToEdit2.orbitalZMultiplier = 10;
                particlesToEdit3.startLifetimeMultiplier = 2;
                particlesToEdit3.simulationSpeed = 2;
                coreChargeParticleLevel = 5;
            }
            
        }
        private void InitializeArtilleryStrikeFX(EventTypes.Event1Param context)
        {
            

            switch (context.fx)
            {
                case FXList.FXlist.ArtilleryStrike:
                    Debug.Log("FXHandler TEST2");
                    StartCoroutine(DelayedStart(context.color, context.position, context.delay, context.duration, ArtilleryStrikePrefab));
                    
                    break;
                case FXList.FXlist.Electricity1:
                    break;
                //var ActualElectricity = Instantiate(ElectricityPrefab, pos, quaternion.identity);
            }       
            
        }

        private void InitializeElectricityFX(EventTypes.Event2Param context)
        {
            StartCoroutine(DelayedStart(context));
        }
        private void ActiveFX()
        {
            
        }
        /// <summary>
        /// DelayedStart for ArtilleryStrikeFX
        /// </summary>
        /// <param name="color"></param>
        /// <param name="pos"></param>
        /// <param name="delay"></param>
        /// <param name="duration"></param>
        /// <param name="FX"></param>
        /// <returns></returns>
        private IEnumerator DelayedStart(Color color, Vector3 pos, float delay, float duration, GameObject FX)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay); 
            }
            
            var realFXObject = Instantiate(FX, pos, quaternion.identity);
            ParticleSystem.MainModule particleSettings = realFXObject.GetComponent<ParticleSystem>().main;
            particleSettings.startColor = new ParticleSystem.MinMaxGradient(color);
            StartCoroutine(TerminateFX(duration, realFXObject));
        } 
        
        /// <summary>
        /// DelayedStart for Electricity FX
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private IEnumerator DelayedStart(EventTypes.Event2Param context)
        {
            if (context.delay > 0)
            {
                yield return new WaitForSeconds(context.delay); 
            }
            
            var realFXObject = Instantiate(ElectricityPrefab1, context.transform);
            ParticleSystem.MainModule particleSettings = realFXObject.GetComponent<ParticleSystem>().main;
            particleSettings.startColor = new ParticleSystem.MinMaxGradient(context.color);
            
            StartCoroutine(TerminateFX(context.duration, realFXObject));
            
            var realFXObject2 = Instantiate(ElectricityPrefab1, context.transform);
            ParticleSystem.MainModule particleSettings2 = realFXObject2.GetComponent<ParticleSystem>().main;
            particleSettings2.startColor = new ParticleSystem.MinMaxGradient(context.color);
            particleSettings2.startSpeed = 2;
            particleSettings2.simulationSpeed = 2;
            
            StartCoroutine(TerminateFX(context.duration, realFXObject2));
        } 
        private IEnumerator TerminateFX(float duration, GameObject particles) 
        {
            
                if (duration > 0) { yield return new WaitForSeconds(duration); }

                Destroy(particles);
                
                

        }
    }
}