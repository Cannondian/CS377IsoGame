using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace RPGCharacterAnims.Actions
{
    
    public class FXHandler : Singleton<FXHandler>
    {
        #region Fields

        

        #endregion

        #region FXInstances

        public GameObject ArtilleryStrikePrefab;
        
        
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
            Skill.OnParticleEffectTriggered += InitializeFX;
        }

        private void OnDisable()
        {
            Skill.OnParticleEffectTriggered -= InitializeFX;
        }
        // PH is for placeholder
        private void SetFlags(string color, Vector3 pos, FXList.FXlist effect, float duration)
        {

        }

        private void InitializeFX(string color, Vector3 pos, FXList.FXlist effect, float duration)
        {
            Debug.Log(FindObjectsOfType(typeof(FXHandler)));

            var ActualArtilleryStrike = Instantiate(ArtilleryStrikePrefab, pos, quaternion.identity);
            StartCoroutine(TerminateFX(duration, ActualArtilleryStrike));
            
            
            
        }

        private void ActiveFX()
        {
            
        }

        private IEnumerator TerminateFX(float duration, GameObject particles) 
        {
            
                if (duration > 0) { yield return new WaitForSeconds(duration); }

                Destroy(particles);
                
                

        }
    }
}