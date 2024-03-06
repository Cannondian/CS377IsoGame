using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using HighlightPlus;
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

        private UnityAction<EventTypes.SkillUsedParam> InitializeFXListener;
        private UnityAction<EventTypes.FlamethrowerStartFXParam> FlamethrowerFXListener;
        private UnityAction<FXList.FXlist> FlamethrowerFXEndListener;

        private UnityAction<EventTypes.Event8Param> VariableFXListener;
        private UnityAction<EventTypes.HitFXParam> HitFXListener;

        private UnityAction<EventTypes.StatusConditionFXParam> NewStatusFXListener;
        private UnityAction<EventTypes.StatusConditionFXParam> ExpiredStatusFXListener;

        private UnityAction<EventTypes.AttackSwingParam> AttackSwingListener;

        private UnityAction<EventTypes.HighlightFXParam> NewHighlightListener;
        private UnityAction<EventTypes.HighlightFXParam> ExpiredHighlightListener;
        #endregion

        #region Fields

        [SerializeField] private HighlightEffect playerHighlightComponent;
        public int coreChargeParticleLevel;

        public Dictionary<GameObject, List<GameObject>> ActiveStatusEffectsDict =
            new Dictionary<GameObject, List<GameObject>>();

        public Dictionary<GameObject, List<GameObject>> StatusEffectsOnQueue =
            new Dictionary<GameObject, List<GameObject>>();

        private HighlightProfile activeHighlight;

    #endregion

        #region FXIPrefabs

        public GameObject ArtilleryStrikePrefab;
        public GameObject ElectricityPrefab1;
        [FormerlySerializedAs("BasicHitPrefab")] [FormerlySerializedAs("BasicHit2Prefab")] public GameObject StaffSwingHitPrefab;
        [FormerlySerializedAs("EnhancedBasicHitPrefab")] [FormerlySerializedAs("EnhancedBasicHit2")] public GameObject KickPrefab;
        public GameObject BurningFXPrefabEnemy;
        
        [FormerlySerializedAs("CorrosiveFXPrefab")] public GameObject PoisonedFXPrefab;
        public GameObject BleedingFXPrefab;
        public GameObject ConfusedFXPrefab;
        public GameObject StunnedFXPrefab;
        public GameObject SlowFXPrefab;
        public GameObject InnerFireFXPrefab;
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
        public GameObject ChlorophyllInfusionPrefab;
        public GameObject AttackArcPrefab;
        public GameObject InnerFireHitFX;
        public GameObject FlamethrowerPrefab;
        public GameObject ILFlamethrowerPrefab;
        public GameObject SHFlamethrowerPrefab;
        public GameObject ZZFlamethrowerPrefab;
        public GameObject VHFlamethrowerPrefab;
        public GameObject OBFlamethrowerPrefab;
        
        
        public GameObject ILMinePrefab;
        public GameObject SHMinePrefab;
        public GameObject ZZMinePrefab;
        public GameObject VHMinePrefab;
        public GameObject OBMinePrefab;

        #endregion

        #region Highlights

        [FormerlySerializedAs("Mech")] public HighlightProfile Zulzara;
        [FormerlySerializedAs("Fire")] public HighlightProfile Ilsihre;
        [FormerlySerializedAs("Air")] public HighlightProfile Shalharan;
        [FormerlySerializedAs("Plant")] public HighlightProfile Velheret;
        [FormerlySerializedAs("Stone")] public HighlightProfile Obhalas;

        #endregion
        
        #region FXInstances
        
        public GameObject coreChargeParticles;
        public GameObject flamethrowerInstance;
        public GameObject playerBurningFX;
        
        #endregion

        [SerializeField] private Rigidbody staffBody;
        public bool alreadyPlaying;
        

        // Update is called once per frame
        void LateUpdate()
        {
            
        }

        private void OnEnable()
        {
            
            InitializeFXListener += InitializeArtilleryStrikeFX;
            EventBus.StartListening(EventTypes.Events.ON_PARTICLE_FX_FOR_SKILL, InitializeFXListener);
            FlamethrowerFXListener += InitializeFlamethrowerFX;
            EventBus.StartListening(EventTypes.Events.ON_FLAMETHROWER_SKILL_START,
                FlamethrowerFXListener);
            FlamethrowerFXEndListener += TerminateFX;
            EventBus.StartListening(EventTypes.Events.ON_FLAMETHROWER_SKILL_END, FlamethrowerFXEndListener) ;
            VariableFXListener += UpdateCoreChargeFX;
            EventBus.StartListening(EventTypes.Events.ON_UPDATE_CORE_CHARGE_PARTICLES, VariableFXListener);
            HitFXListener += InitializeHitFX;
            EventBus.StartListening(EventTypes.Events.ON_BASIC_ATTACK_HIT, HitFXListener);
            NewStatusFXListener += InitializeStatusConditionFX;
            EventBus.StartListening(EventTypes.Events.ON_NEW_STATUS_CONDITION, NewStatusFXListener);
            ExpiredStatusFXListener += TerminateStatusConditionFX;
            EventBus.StartListening(EventTypes.Events.ON_EXPIRED_STATUS_CONDITION, ExpiredStatusFXListener);
            AttackSwingListener += EnableAttackSwingFX;
            EventBus.StartListening(EventTypes.Events.ON_ATTACK_SWING, AttackSwingListener);
            NewHighlightListener += InitializeTileStateHighlight;
            EventBus.StartListening(EventTypes.Events.ON_HIGHLIGHT_FX_TRIGGERED, NewHighlightListener);
            ExpiredHighlightListener += KillTileStateHighlight;
            EventBus.StartListening(EventTypes.Events.ON_HIGHLIGHT_FX_EXPIRED, ExpiredHighlightListener);
        }

        private void OnDisable()
        {
           EventBus.StopListeningAll<EventTypes.Events>();
        }
        // PH is for placeholder
        private void SetFlags(string color, Vector3 pos, FXList.FXlist effect, float duration)
        {

        }

        private void EnableAttackSwingFX(EventTypes.AttackSwingParam context)
        {
            StartCoroutine(AttackSwingDelay(context));
        }

        private IEnumerator AttackSwingDelay(EventTypes.AttackSwingParam context)
        {
            if (context.attackType == 1)
            {
                yield return new WaitForSeconds(0.3f / context.attackSpeed);
                AttackArcPrefab.SetActive(true);
                AttackArcPrefab.transform.rotation = Quaternion.Euler(staffBody.angularVelocity.normalized);
                yield return new WaitForSeconds(0.65f / context.attackSpeed);
                AttackArcPrefab.SetActive(false);
            }
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
                particlesToEdit2.orbitalXMultiplier = 5;
                particlesToEdit2.orbitalYMultiplier = 5;
                particlesToEdit2.orbitalZMultiplier = 5;
                particlesToEdit3.startLifetimeMultiplier = 1.2f;
                particlesToEdit3.simulationSpeed = 1.3f;
                coreChargeParticleLevel = 2;
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(2, 12);
            }
            else if (context.coreCharge <= 30)
            {
                particlesToEdit.dragMultiplier = 1;
                particlesToEdit2.orbitalXMultiplier = 6;
                particlesToEdit2.orbitalYMultiplier = 6;
                particlesToEdit2.orbitalZMultiplier = 6;
                particlesToEdit3.startLifetimeMultiplier = 1.4f;
                particlesToEdit3.simulationSpeed = 1.5f;
                coreChargeParticleLevel = 3;
                var burst = particlesToEdit4.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(4, 16);
                
            }
            else if (context.coreCharge <= 40)
            {
                particlesToEdit.dragMultiplier = 0.76f;
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
                particlesToEdit.dragMultiplier = 0.4f;
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

        private void InitializeArtilleryStrikeFX(EventTypes.SkillUsedParam context)
        {



            switch (context.tile)
            {
                case TileElement.ElementType.Ilsihre:
                    StartCoroutine(DelayedStart(context.position, context.delay, context.duration, ILMinePrefab));
                    break;
                case TileElement.ElementType.Velheret:
                    StartCoroutine(DelayedStart(context.position, context.delay, context.duration, VHMinePrefab));
                    break;
                case TileElement.ElementType.Shalharan:
                    StartCoroutine(DelayedStart(context.position, context.delay, context.duration, SHMinePrefab));
                    break;
                case TileElement.ElementType.None:
                    StartCoroutine(DelayedStart(context.position, context.delay, context.duration, ArtilleryStrikePrefab));
                    break;
            }



        }

        private void InitializeTileStateHighlight(EventTypes.HighlightFXParam context)
        {
            
            
            activeHighlight = context.tileHighlight;

            playerHighlightComponent.profile = context.tileHighlight;
            playerHighlightComponent.highlighted = true;
            playerHighlightComponent.ProfileReload();
            
            


        }

        private void KillTileStateHighlight(EventTypes.HighlightFXParam context)
        {
            if (context.tileHighlight == activeHighlight)
            {
               
                playerHighlightComponent.highlighted = false;
                playerHighlightComponent.ProfileReload();
                
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
                
                GameObject statusConditionFX = Instantiate(context.condition, context.caller.transform.position, 
                    quaternion.identity, context.caller.transform );
                statusConditionFX.transform.localPosition = new Vector3(0, -0.2f, 0);
                statusConditionFX.name = context.myName;
                List<GameObject> objectActiveStatusFX = new List<GameObject>();
                objectActiveStatusFX.Add(statusConditionFX);
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
                Debug.Log(activeFX[0]);
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
                    if (StatusEffectsOnQueue.TryGetValue(context.caller, out var FXonQueueForCaller))
                    {
                        if (FXonQueueForCaller.Count != 0)
                        {
                            GameObject nextFX = FXonQueueForCaller[0];
                            nextFX.SetActive(true);
                            activeFX.Add(nextFX);
                        
                        }
                    }
                    

                }
            }
            else
            {
                Debug.Log("something went wrong, you are trying to remove an FX that's not there!");
            }
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
        private IEnumerator DelayedStart( Vector3 pos, float delay, float duration, GameObject FX)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay); 
            }
            
            var realFXObject = Instantiate(FX, pos, quaternion.identity);
            
            
            StartCoroutine(DelayedTerminateFX(duration, realFXObject));
        } 
        
        /// <summary>
        /// DelayedStart for Electricity FX
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        

        private void InitializeHitFX(EventTypes.HitFXParam context)
        {
            StartCoroutine(DelayedStartForHitFX(context));
        }

        private void InitializeFlamethrowerFX(EventTypes.FlamethrowerStartFXParam context)
        {
            switch (context.element)
            {
                case TileElement.ElementType.Ilsihre:
                    flamethrowerInstance = Instantiate(ILFlamethrowerPrefab, context.transform);
                    break;
                case TileElement.ElementType.Shalharan:
                    flamethrowerInstance = Instantiate(SHFlamethrowerPrefab, context.transform);
                    break;
                case TileElement.ElementType.Zulzara:
                    flamethrowerInstance = Instantiate(ZZFlamethrowerPrefab, context.transform);
                    break;
                case TileElement.ElementType.Velheret:
                    flamethrowerInstance = Instantiate(VHFlamethrowerPrefab, context.transform);
                    break;
                case TileElement.ElementType.Obhalas:
                    flamethrowerInstance = Instantiate(OBFlamethrowerPrefab, context.transform);
                    break;
                case TileElement.ElementType.None:
                    flamethrowerInstance = Instantiate(FlamethrowerPrefab, context.transform);
                    break;
               
            }
            
            
        }

        private void TerminateFX(FXList.FXlist fx)
        {
            if (fx == FXList.FXlist.Flamethrower)
            {
                Destroy(flamethrowerInstance);
            }
        }
        private IEnumerator DelayedTerminateFX(float duration, GameObject particles) 
        {
            
                if (duration > 0) { yield return new WaitForSeconds(duration); }

                Destroy(particles);
                alreadyPlaying = false;

        }

        private IEnumerator DelayedStartForHitFX(EventTypes.HitFXParam context)
        {
            
            if (context.attackType == 1 || context.attackType == 2)
            {
                yield return new WaitForSeconds(0.15f);
                if (context.attackType == 1)
                {
                    
                    var hitObject = Instantiate(StaffSwingHitPrefab, context.hitPosition,
                        context.hitter.transform.rotation);
                    DelayedTerminateFX(0.5f, hitObject);
                }
                else
                {
                    var attackRotation = Quaternion.Euler(-51, 166, 176);
                    var hitObject = Instantiate(StaffSwingHitPrefab, context.hitPosition,
                        attackRotation);
                    DelayedTerminateFX(0.5f, hitObject);
                }
                

                

            }
            else if (context.attackType == 5)
            {
                
                var hitObject2 = Instantiate(KickPrefab, context.hitPosition,
                    context.hitter.transform.rotation);
                hitObject2.transform.position = context.hitPosition;
                DelayedTerminateFX(0.5f, hitObject2);
                    
            }
            else
            {
                var hitObject3 = Instantiate(InnerFireHitFX, context.hitPosition,
                    context.hitter.transform.rotation);
                hitObject3.transform.position = context.hitPosition;
                DelayedTerminateFX(0.5f, hitObject3);
            }
        }
    }
}