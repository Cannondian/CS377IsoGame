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


namespace RPGCharacterAnims.Actions
{
    
    public class FXHandler : Singleton<FXHandler>
    {
        #region Delegates

        private UnityAction<EventTypes.Event1Param> InitializeFXListener;
        private UnityAction<EventTypes.Event2Param> ContinousFXListener;

        #endregion
        
        #region Fields

        

        #endregion

        #region FXInstances

        public GameObject ArtilleryStrikePrefab;
        public GameObject ElectricityPrefab1;
        public GameObject ElectricityPrefab2;
        
        
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
        }

        private void OnDisable()
        {
           EventBus.StopListening(EventTypes.Events.ON_PARTICLE_FX_TRIGGER, InitializeFXListener);
        }
        // PH is for placeholder
        private void SetFlags(string color, Vector3 pos, FXList.FXlist effect, float duration)
        {

        }

        private void InitializeArtilleryStrikeFX(EventTypes.Event1Param context)
        {
            

            switch (context.fx)
            {
                case FXList.FXlist.ArtilleryStrike:
                    Debug.Log("FXHandler TEST2");
                    StartCoroutine(DelayedStart(context.color, context.position, context.delay, context.duration, ArtilleryStrikePrefab));
                    
                    break;
                case FXList.FXlist.Electricity:
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