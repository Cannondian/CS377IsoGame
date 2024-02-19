using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AuraOutlines
{
    [CustomEditor(typeof(AuraOutlinesFeature))]
    public class StylizedOutlinesFeatureEditor : Editor
    {
        SerializedProperty UseDepthMask;
        SerializedProperty layerMask;

        SerializedObject auraOutlineSettings;

        void OnEnable()
        {
            UseDepthMask = serializedObject.FindProperty("UseDepthMask");
            layerMask = serializedObject.FindProperty("layerMask");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
            EditorGUILayout.PropertyField(UseDepthMask);
            EditorGUILayout.PropertyField(layerMask);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("auraOutlinesSettings"));
            if(serializedObject.FindProperty("auraOutlinesSettings").objectReferenceValue)
                auraOutlineSettings = new SerializedObject(serializedObject.FindProperty("auraOutlinesSettings").objectReferenceValue);
            else
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }

            AuraOutlinesFeature myTarget = (AuraOutlinesFeature)target;
            if(auraOutlineSettings.UpdateIfRequiredOrScript()) myTarget.Create();

            EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("UseColorGradient"));
            if (auraOutlineSettings.FindProperty("UseColorGradient").boolValue)
            {
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("ColorGradient"));
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("ColorGradientSpeed"));
            }
            else EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("Color"));

            EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("Width"));

            EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("UseNoiseAlpha"));
            if (auraOutlineSettings.FindProperty("UseNoiseAlpha").boolValue)
            {
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("NoiseAlpha"));
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("NoiseScaleAlpha"));
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("MultiplierAlpha"));
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("SpeedXAlpha"));
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("SpeedYAlpha"));
            }
            else  EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("DefaultNoiseAlpha"));

            EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("UseNoiseColor"));
            if (auraOutlineSettings.FindProperty("UseNoiseColor").boolValue)
            {
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("UseNoiseColorGradient"));
                if (auraOutlineSettings.FindProperty("UseNoiseColorGradient").boolValue)
                {
                    EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("ColorNoiseGradient"));
                    EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("ColorNoiseGradientSpeed"));
                }
                else EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("NoiseTintColor"));


                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("NoiseColor"));
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("NoiseScaleColor"));

                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("NoiseIntensity"));



               
                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("RandomizeUvs"));
                if (auraOutlineSettings.FindProperty("RandomizeUvs").boolValue)
                {
                    EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("RandomizationSpeedFactor"));
                }

                EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("UseFlowColor"));
                if (auraOutlineSettings.FindProperty("UseFlowColor").boolValue)
                {
                    EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("SpeedXColor"));
                    EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("SpeedYColor"));
                }


            }

            EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("ColorMidStrength"));
            EditorGUILayout.PropertyField(auraOutlineSettings.FindProperty("ColorAlwaysAbove1"));


            auraOutlineSettings.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
}