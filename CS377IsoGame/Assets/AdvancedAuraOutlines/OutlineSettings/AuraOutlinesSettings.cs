using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuraOutlines
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "AuraOutlines/AuraOutlinesSettings", order = 1)]
    public class AuraOutlinesSettings : ScriptableObject
    {
        [Header("Main")]
        [SerializeField] public bool UseColorGradient = false;
        [SerializeField] public Gradient ColorGradient;
        [SerializeField] public float ColorGradientSpeed;

        [SerializeField, ColorUsage(true, true)] public Color Color = Color.green;
        [SerializeField, Range(0, 32)] private float Width = 10;

        [Header("Alpha channel")]
        [SerializeField] private bool UseNoiseAlpha = false;
        [SerializeField] private Texture2D NoiseAlpha;
        [SerializeField] private float NoiseScaleAlpha = 1;
        [SerializeField, Range(-20, 20)] private float SpeedXAlpha = 0;
        [SerializeField, Range(-20, 20)] private float SpeedYAlpha = 0;
        [SerializeField, Range(0, 0.15f)] private float MultiplierAlpha = 0.015f;

        [Header("Color channel")]
        [SerializeField] private bool UseNoiseColor = false;
        [SerializeField,Range(0,0.02f)] private float DefaultNoiseAlpha = 0.008f;

        [SerializeField] public bool UseNoiseColorGradient = false;
        [SerializeField] public Gradient ColorNoiseGradient;
        [SerializeField] public float ColorNoiseGradientSpeed;

        [SerializeField, ColorUsage(true, true)] private Color NoiseTintColor = Color.blue;
        [SerializeField, Range(0, 5)] private float NoiseIntensity = 1;

        [SerializeField] private bool RandomizeUvs = false;
        [SerializeField] private float RandomizationSpeedFactor = 10;

        [SerializeField] private bool UseFlowColor = false;
        [SerializeField, Range(-20, 20)] private float SpeedXColor = 0;
        [SerializeField, Range(-20, 20)] private float SpeedYColor = 0;

        [SerializeField] private Texture2D NoiseColor;
        [SerializeField] private float NoiseScaleColor = 1;
        [SerializeField, Range(0, 0.05f)] private float ColorMidStrength = 0.01f;

        [Header("Other")]
        [SerializeField] private bool ColorAlwaysAbove1 = true;


        public string GetShaderName()
        {
            return "Aura/Aura";
        }

        public void SetupShaderProperties(Material material)
        {
            //Main
            material.SetColor("_Color", Color);
            material.SetFloat("_Width", Width);

            //Alpha channel
            material.SetFloat("_UseNoiseAlpha", UseNoiseAlpha ? 1 : 0);
            material.SetTexture("_NoiseTexAlpha", NoiseAlpha);
            material.SetFloat("_ScaleAlpha", NoiseScaleAlpha);
            material.SetFloat("_SpeedXAlpha", SpeedXAlpha);
            material.SetFloat("_SpeedYAlpha", SpeedYAlpha);
            material.SetFloat("_MultiplierAlpha", MultiplierAlpha);

            //Color channel
            material.SetFloat("_UseNoiseColor", UseNoiseColor ? 1 : 0);
            material.SetFloat("_UseRandomizationColor", RandomizeUvs ? 1 : 0);
            material.SetFloat("_RandomizationSpeedColor", RandomizationSpeedFactor);

            material.SetFloat("_UseFlowColor", UseFlowColor ? 1 : 0);
            material.SetFloat("_SpeedXColor", SpeedXColor);
            material.SetFloat("_SpeedYColor", SpeedYColor);

            material.SetFloat("_DefaultNoiseAlpha", DefaultNoiseAlpha);
            material.SetTexture("_NoiseTexColor", NoiseColor);
            material.SetFloat("_ScaleColor", NoiseScaleColor);
            material.SetColor("_NoiseTintColor", NoiseTintColor);
            material.SetFloat("_ColorMidStrength", ColorMidStrength);
            material.SetFloat("_ColorNoiseIntensity", NoiseIntensity);
            material.SetFloat("_ColorAlwaysAbove1", ColorAlwaysAbove1 ? 1 : 0);
        }
    }
}