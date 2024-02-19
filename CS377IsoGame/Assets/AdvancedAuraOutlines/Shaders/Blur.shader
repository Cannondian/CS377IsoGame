Shader "Aura/Blur"
{
	Properties
	{
		//_MainTex ("Base (RGB)", 2D) = "" {}
		//_SourceTex("ss", 2D) = "" {}
		//_RandomName("s", 2D) = "" {}
	}

	HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			TEXTURE2D(_CameraColorTexture);
			SAMPLER(sampler_CameraColorTexture);

			TEXTURE2D (_EnlargedObjectsTex);
			SAMPLER (sampler_EnlargedObjectsTex);
			
			TEXTURE2D (_AfterVPassTex);
			SAMPLER (sampler_AfterVPassTex);

		CBUFFER_START(UnityPerMaterial)
			float2 _CameraColorTexture_TexelSize;

			int _Width;
			float _GaussSamples[32];
			float _Intensity;
			float4 _Color;
		CBUFFER_END

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
		
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings VertexSimple(Attributes input)
			{
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
				output.uv = input.uv;

				return output;
			}

            float CalcIntensityN0(float2 uv, float2 offset, int k, Texture2D tex,sampler sam)
			{
				return SAMPLE_TEXTURE2D(tex, sam, uv + k * offset).r * _GaussSamples[k];
			}

			float CalcIntensityN1(float2 uv, float2 offset, int k, Texture2D tex,sampler sam)
			{
				return SAMPLE_TEXTURE2D(tex, sam, uv - k * offset).r * _GaussSamples[k];
			}

			float CalcIntensity(float2 uv, float2 offset, Texture2D tex,sampler sam)
			{
				float intensity = 0;

				// Accumulates horizontal or vertical blur intensity for the specified texture position.
				// Set offset = (tx, 0) for horizontal sampling and offset = (0, ty) for vertical.

				[unroll(32)]
				for (int k = 1; k <= _Width; ++k)
				{
					intensity += CalcIntensityN0(uv, offset, k,tex,sam);
					intensity += CalcIntensityN1(uv, offset, k,tex,sam);
				}

				intensity += CalcIntensityN0(uv, offset, 0,tex,sam);
				return intensity;
			}

			float4 FragmentH(Varyings i) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				float2 uv = UnityStereoTransformScreenSpaceTex(i.uv);	
				float intensity = CalcIntensity(uv, float2(_CameraColorTexture_TexelSize.x, 0),_EnlargedObjectsTex,sampler_EnlargedObjectsTex);
				return float4(intensity, intensity, intensity, 1);
			}

			float4 FragmentV(Varyings i) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				float2 uv = UnityStereoTransformScreenSpaceTex(i.uv);
				float intensity = CalcIntensity(uv, float2(0, _CameraColorTexture_TexelSize.y),_AfterVPassTex,sampler_AfterVPassTex);

				intensity = _Intensity > 99 ? step(0.01, intensity) : intensity * _Intensity;

				float4 mask = SAMPLE_TEXTURE2D(_EnlargedObjectsTex, sampler_EnlargedObjectsTex, uv);
				if (mask.r > 0)
				{
	    			return SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);
				}

				return float4(_Color.rgb, saturate(_Color.a * intensity)) + SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);

			}

	ENDHLSL

	SubShader
    {
		Tags { "RenderPipeline" = "UniversalPipeline" }
		ZWrite Off
		Lighting Off

        Pass
        {
			Name "HPass"

            HLSLPROGRAM

			#pragma target 3.5
			#pragma multi_compile_instancing
			#pragma vertex VertexSimple
			#pragma fragment FragmentH

			ENDHLSL
        }

		Pass
        {
            Name "VPassBlend"
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM

			#pragma target 3.5
			#pragma multi_compile_instancing
			#pragma vertex VertexSimple
			#pragma fragment FragmentV

			ENDHLSL
        }
    }
}
