using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

namespace AuraOutlines
{
    public class AuraPass : ScriptableRenderPass
    {
        private readonly Material material;
        private RenderTargetIdentifier cameraColorTarget;

        private RenderTargetHandle temporaryBuffer;

        private int MaxWidth = 32;
        private float[] gaussSamples;

        private readonly bool UseColorGradient = false;
        private readonly Gradient ColorGradient;
        private readonly float ColorGradientSpeed;

        private readonly bool UseNoiseTintGradient = false;
        private readonly Gradient NoiseTintGradient;
        private readonly float NoiseTintGradientSpeed;

        private float GradientTime = 0;

        private float[] GetGaussSamples(int width, float[] samples)
        {
            // NOTE: According to '3 sigma' rule there is no reason to have StdDev less then width / 3.
            // In practice blur looks best when StdDev is within range [width / 3,  width / 2].
            var stdDev = width * 0.5f;

            if (samples is null)
            {
                samples = new float[MaxWidth];
            }

            for (var i = 0; i < width; i++)
            {
                samples[i] = Gauss(i, stdDev);
            }

            return samples;
        }

        private float Gauss(float x, float stdDev)
        {
            var stdDev2 = stdDev * stdDev * 2;
            var a = 1 / Mathf.Sqrt(Mathf.PI * stdDev2);
            var gauss = a * Mathf.Pow((float)Math.E, -x * x / stdDev2);

            return gauss;
        }

        public AuraPass(RenderPassEvent renderPassEvent, AuraOutlinesSettings auraSettings,int number)
        {
            if (!auraSettings) return;
            UseColorGradient = auraSettings.UseColorGradient;
            ColorGradient = auraSettings.ColorGradient;
            ColorGradientSpeed = auraSettings.ColorGradientSpeed;

            UseNoiseTintGradient = auraSettings.UseNoiseColorGradient;
            NoiseTintGradient = auraSettings.ColorNoiseGradient;
            NoiseTintGradientSpeed = auraSettings.ColorNoiseGradientSpeed;

            this.renderPassEvent = renderPassEvent;

            material = new Material(Shader.Find(auraSettings.GetShaderName()));
            auraSettings.SetupShaderProperties(material);

            material.EnableKeyword("_Tex" + number.ToString());

            gaussSamples = GetGaussSamples(32, gaussSamples);
            material.SetFloatArray("_GaussSamples", gaussSamples);

            temporaryBuffer.Init("_AfterVPassTex" + number.ToString());
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!material) return;

            GradientTime += Time.deltaTime / 1000 * ColorGradientSpeed;
            GradientTime = GradientTime % 1;

            if (UseColorGradient) material.SetColor("_Color", ColorGradient.Evaluate(GradientTime));
            if (UseNoiseTintGradient) material.SetColor("_NoiseTintColor", NoiseTintGradient.Evaluate(Time.realtimeSinceStartup * NoiseTintGradientSpeed));

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(
                "_AuraOutlinesBlit")))
            {
                RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;

                cmd.GetTemporaryRT(temporaryBuffer.id, opaqueDescriptor, FilterMode.Point);

                Blit(cmd, cameraColorTarget, temporaryBuffer.Identifier(), material, 0);
                Blit(cmd, temporaryBuffer.Identifier(), cameraColorTarget, material, 1);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryBuffer.id);
        }
    }

}
