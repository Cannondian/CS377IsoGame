using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Rendering;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;


namespace RPGCharacterAnims.Actions
{

    public class FXHandler : Singleton<FXHandler>
    {
        #region Delegates

        private UnityAction<EventTypes.Event1Param> InitializeFXListener;
        private UnityAction<EventTypes.Event2Param> ContinousFXListener;


        private UnityAction<EventTypes.Event8Param> VariableFXListener;
        private UnityAction<EventTypes.Event9Param> HitFXListener;

        private UnityAction<EventTypes.StatusConditionFXParam> NewStatusFXListener;
        private UnityAction<EventTypes.StatusConditionFXParam> ExpiredStatusFXListener;

        #endregion

        #region Fields


        public int coreChargeParticleLevel;

        public Dictionary<GameObject, List<GameObject>> ActiveStatusEffectsDict =
            new Dictionary<GameObject, List<GameObject>>();

        public Dictionary<GameObject, List<GameObject>> StatusEffectsOnQueue =
            new Dictionary<GameObject, List<GameObject>>();

    #endregion

        #region FXIPrefabs

        public GameObject ArtilleryStrikePrefab;
        public GameObject ElectricityPrefab1;
        [FormerlySerializedAs("BasicHit2Prefab")] public GameObject BasicHitPrefab;
        [FormerlySerializedAs("EnhancedBasicHit2")] public GameObject EnhancedBasicHitPrefab;
        public GameObject BurningFXPrefab;
        public GameObject CorrosiveFXPrefab;
        public GameObject BleedingFXPrefab;
        public GameObject ConfusedFXPrefab;
        public GameObject StunnedFXPrefab;
        public GameObject SlowFXPrefab;
        public GameObject ExhaustedFXPrefab;
        public GameObject RejuvenationFXPrefab;
        public GameObject EnergizedFXPrefab;
        public GameObject SlipperyStepsFXPrefab;
        public GameObject SmolderingStrikesFXPrefab;
        public GameObject EvasiveFXPrefab;
        public GameObject DefensiveTerrainFXPrefab;
        public GameObject RadiationPoisoningFXPrefab;
        public GameObject MutatingFXPrefab;
        public GameObject GlowingFXPrefab; 
        

        

        #endregion

        #region FXInstances


        public GameObject coreChargeParticles;
        
        #endregion

        public bool alreadyPlaying;
        
        

        // Update is called once per frame
        void LateUpdate()
        {
            
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
            HitFXListener += InitializeHitFX;
            EventBus.StartListening(EventTypes.Events.ON_BASIC_ATTACK_HIT, HitFXListener);
            NewStatusFXListener += InitializeStatusConditionFX;
            EventBus.StartListening(EventTypes.Events.ON_NEW_STATUS_CONDITION, NewStatusFXListener);
            ExpiredStatusFXListener += TerminateStatusConditionFX;
            EventBus.StartListening(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION, ExpiredStatusFXListener);

        }

        private void OnDisable()
        {
           EventBus.StopListening(EventTypes.Events.ON_PARTICLE_FX_TRIGGER, InitializeFXListener);
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
            var particlesToEdit4 = coreChargeParticles.GetComponent<ParticleSystem>().emission;
            if (context.coreCharge == 0)
            {
                particlesToEdit.dragMultiplier = 5;
                coreChargeParticleLevel = 0;
            }
            else if (context.coreCharge <= 10)
            {
                particlesToEdit.dragMultiplier = 1.4f;
                coreChargeParticleLevel = 1;
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(0, 8);
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
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(0, 12);
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
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(0, 16);
                
            }
            else if (context.coreCharge <= 40)
            {
                particlesToEdit.dragMultiplier = 0.7f;
                particlesToEdit2.orbitalXMultiplier = 8;
                particlesToEdit2.orbitalYMultiplier = 8;
                particlesToEdit2.orbitalZMultiplier = 8;
                particlesToEdit3.startLifetimeMultiplier = 1.6f;
                particlesToEdit3.simulationSpeed = 1.8f;
                coreChargeParticleLevel = 4;
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(6, 24);
                
            }
            else
            {
                particlesToEdit.dragMultiplier = 0.6f;
                particlesToEdit2.orbitalXMultiplier = 10;
                particlesToEdit2.orbitalYMultiplier = 10;
                particlesToEdit2.orbitalZMultiplier = 10;
                particlesToEdit3.startLifetimeMultiplier = 2;
                particlesToEdit3.simulationSpeed = 2;
                coreChargeParticleLevel = 5;
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(10, 30);
                
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
                case FXList.FXlist.ElectricityFX1:
                    break;
                //var ActualElectricity = Instantiate(ElectricityPrefab, pos, quaternion.identity);
            }       
            
        }

        /// <summary>
        /// checks to see if the game object has 3 active particle effects on it, if not instantiates it and adds it to the active FX list
        /// if so, adds the prefab to the queue of uninstantiated yet active particle effects. If there is queue for the object yet, makes one.
        /// if the game object has no status conditions on it yet, adds a list of activeFX for it to the activeFX dictionary
        /// </summary>
        /// <param name="context"></param>
        private void InitializeStatusConditionFX(EventTypes.StatusConditionFXParam context)
        {
            //invariant here is that the StatusConditionState class doesn't send duplicates for any given effect
            //before removing the previous one
           
            if (ActiveStatusEffectsDict.TryGetValue(context.caller, out var activeStatusFX))
            {
                
                if (activeStatusFX.Count < 3)
                {
                    
                    GameObject statusConditionFX = Instantiate(context.condition, context.caller.transform);
                    statusConditionFX.name = context.myName;
                    activeStatusFX.Add(statusConditionFX);
                }
                else if (StatusEffectsOnQueue.TryGetValue(context.caller, out var queuedStatusFX)) //is the game object in the dictionary for queued status FX?
                {
                    GameObject FXtoPutOnQueue = Instantiate(context.condition, context.caller.transform);
                    FXtoPutOnQueue.name = context.myName;
                    FXtoPutOnQueue.SetActive(false);
                    queuedStatusFX.Add(FXtoPutOnQueue);
                }
                else
                {
                    List<GameObject> objectQueuedStatusFX = new List<GameObject>();
                    GameObject FXtoPutOnQueue = Instantiate(context.condition, context.caller.transform);
                    FXtoPutOnQueue.name = context.myName;
                    FXtoPutOnQueue.SetActive(false);
                    objectQueuedStatusFX.Add(FXtoPutOnQueue);
                    StatusEffectsOnQueue.Add(context.caller, objectQueuedStatusFX);
                }
            }
            else
            {
                Debug.Log("this is called");
                GameObject statusConditionFX = Instantiate(context.condition, context.caller.transform.position, 
                    quaternion.identity, context.caller.transform );
                statusConditionFX.transform.localPosition = new Vector3(0, -0.2f, 0);
                statusConditionFX.name = context.myName;
                List<GameObject> objectActiveStatusFX = new List<GameObject> { statusConditionFX };
                ActiveStatusEffectsDict.Add(context.caller, objectActiveStatusFX);
            }
        }
        
        /// <summary>
        /// looks for the status condition by first indexing at the active status FX dictionary with the caller game object
        /// if it finds the particle effect its searching for there, it removes it from the list and destroys it
        /// then looks for the next status effect to be instantiated as a visual FX in the queued status effects,
        /// if it finds one, it removes the prefab for the effects from the queue, instantiates it as a new game object,
        /// and adds it to the active status effects list for the given object.
        /// if there is mo status condition on queue, stops.
        /// if the status condition being searched for is not in the active status FX dictionary,
        /// searches for it in the queued status conditions dictionary. If it is there, which it must be, removes it and destroys it.
        /// </summary>
        /// <param name="context"></param>
        private void TerminateStatusConditionFX(EventTypes.StatusConditionFXParam context) 
        {
            if (ActiveStatusEffectsDict.TryGetValue(context.caller, out var activeFX))
            {
                GameObject FXtoTerminate = activeFX.Find(x => x.name == context.myName);
                if (FXtoTerminate == null)
                {
                    List<GameObject> FXonQueueForCaller = StatusEffectsOnQueue[context.caller];
                    FXtoTerminate = FXonQueueForCaller.Find(x => x.name == context.myName);
                    FXonQueueForCaller.Remove(FXtoTerminate);
                    Destroy(FXtoTerminate);
                    if (FXonQueueForCaller.Count == 0)
                    {
                        StatusEffectsOnQueue.Remove(context.caller);
                    }
                }
                else
                {
                    activeFX.Remove(FXtoTerminate);
                    Destroy(FXtoTerminate);
                    List<GameObject> FXonQueueForCaller = StatusEffectsOnQueue[context.caller];
                    if (FXonQueueForCaller.Count != 0)
                    {
                        GameObject nextFX = FXonQueueForCaller[0];
                        nextFX.SetActive(true);
                        activeFX.Add(nextFX);
                        
                    }

                }
            }
            else
            {
                Debug.Log("something went wrong, you are trying to remove an FX that's not there!");
            }
        }

        private void InitializeElectricityFX(EventTypes.Event2Param context)
        {
            StartCoroutine(DelayedStart(context));
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

        private void InitializeHitFX(EventTypes.Event9Param context)
        {
            switch (context.fx)
            {
                case FXList.FXlist.BasicHitFX:
                   
                    var hitObject = Instantiate(BasicHitPrefab, context.hitPosition, context.quaternion);
                        
                    TerminateFX(0.5f, hitObject);
                    

                    break;
                case FXList.FXlist.EnhancedHitFX:
                    var hitObject2 = Instantiate(EnhancedBasicHitPrefab, context.hitPosition, context.quaternion);
                    TerminateFX(0.3f, hitObject2);
                    break;
            }
        }
        
        private IEnumerator TerminateFX(float duration, GameObject particles) 
        {
            
                if (duration > 0) { yield return new WaitForSeconds(duration); }

                Destroy(particles);
                alreadyPlaying = false;



        }
    }
}