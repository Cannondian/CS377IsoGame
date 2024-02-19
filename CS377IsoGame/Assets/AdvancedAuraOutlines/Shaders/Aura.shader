Shader "Aura/Aura"
{
	Properties
	{
		//_MainTex ("Base (RGB)", 2D) = "" {}
		//_SourceTex("ss", 2D) = "" {}
		//_RandomName("s", 2D) = "" {}
	}

	HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			#pragma multi_compile _Tex1 _Tex2 _Tex3 _Tex4 _Tex5

			#ifdef _Tex1
			#define _EnlargedObjectsTex _EnlargedObjectsTex1
			#define sampler_EnlargedObjectsTex sampler_EnlargedObjectsTex1

			#define _AfterVPassTex _AfterVPassTex1
			#define sampler_AfterVPassTex sampler_AfterVPassTex1

			#elif _Tex2
			#define _EnlargedObjectsTex _EnlargedObjectsTex2
			#define sampler_EnlargedObjectsTex sampler_EnlargedObjectsTex2

			#define _AfterVPassTex _AfterVPassTex2
			#define sampler_AfterVPassTex sampler_AfterVPassTex2

			#elif _Tex3
			#define _EnlargedObjectsTex _EnlargedObjectsTex3
			#define sampler_EnlargedObjectsTex sampler_EnlargedObjectsTex3

			#define _AfterVPassTex _AfterVPassTex3
			#define sampler_AfterVPassTex sampler_AfterVPassTex3

			#elif _Tex4
			#define _EnlargedObjectsTex _EnlargedObjectsTex4
			#define sampler_EnlargedObjectsTex sampler_EnlargedObjectsTex4

			#define _AfterVPassTex _AfterVPassTex4
			#define sampler_AfterVPassTex sampler_AfterVPassTex4

			#elif _Tex5
			#define _EnlargedObjectsTex _EnlargedObjectsTex5
			#define sampler_EnlargedObjectsTex sampler_EnlargedObjectsTex5

			#define _AfterVPassTex _AfterVPassTex5
			#define sampler_AfterVPassTex sampler_AfterVPassTex5
			#endif

			TEXTURE2D(_CameraColorTexture);
			SAMPLER(sampler_CameraColorTexture);

			TEXTURE2D (_EnlargedObjectsTex);
			SAMPLER (sampler_EnlargedObjectsTex);
			
			TEXTURE2D (_AfterVPassTex);
			SAMPLER (sampler_AfterVPassTex);

			TEXTURE2D (_NoiseTexAlpha);
			SAMPLER (sampler_NoiseTexAlpha);

			TEXTURE2D (_NoiseTexColor);
			SAMPLER (sampler_NoiseTexColor);

		CBUFFER_START(UnityPerMaterial)
			float2 _CameraColorTexture_TexelSize;

			float4 _Color;
			int _Width;
			float _GaussSamples[32];

			//Depth
			bool _UseBehindWallColor;
			float4 _BehindWallColor;

			//Alpha
			float _UseNoiseAlpha;
			float _MultiplierAlpha; 
			float _ScaleAlpha; 
			float _SpeedXAlpha;
			float _SpeedYAlpha;

			//Color
			float _UseNoiseColor;

			float _UseRandomizationColor;
			float _RandomizationSpeedColor;
			
			float _UseFlowColor;
			float _SpeedXColor;
			float _SpeedYColor;

			float _DefaultNoiseAlpha;
			float4 _NoiseTintColor; 
			float _ScaleColor; 
			float _ColorMidStrength;
			float _ColorAlwaysAbove1;
			float _ColorNoiseIntensity;


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
				return SAMPLE_TEXTURE2D(tex, sam, uv + k * offset).g;// * _GaussSamples[k];
			}

			float CalcIntensityN1(float2 uv, float2 offset, int k, Texture2D tex,sampler sam)
			{
				return SAMPLE_TEXTURE2D(tex, sam, uv - k * offset).g;// * _GaussSamples[k];
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
				
				float4 mask = SAMPLE_TEXTURE2D(_EnlargedObjectsTex, sampler_EnlargedObjectsTex, uv);

				//Object behind wall 
				if (mask.r > 0)
				{
					//if(_UseBehindWallColor)return _BehindWallColor + SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);
				}

				//Object before wall
				if (mask.g == 1)
				{
	    			return SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);
				}

				float intensity = CalcIntensity(uv, float2(0, _CameraColorTexture_TexelSize.y),_AfterVPassTex,sampler_AfterVPassTex);

				//Color
				float2 uvc = float2(uv.x * _ScaleColor, uv.y * _ScaleColor);

				if(_UseRandomizationColor)
				{
					float sinX = sin (round(_Time.x*_RandomizationSpeedColor));
					float cosX = cos (round(_Time.x*_RandomizationSpeedColor));
					float sinY = sin (round(_Time.x*_RandomizationSpeedColor));
					float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
					uvc = mul(uvc,rotationMatrix);
				}

				if(_UseFlowColor)
				{
					uvc -= float2(_Time.x * _SpeedXColor, _Time.x * _SpeedYColor);
				}

				float colorNoiseLerp = 0;
				if(_UseNoiseColor) colorNoiseLerp = SAMPLE_TEXTURE2D(_NoiseTexColor, sampler_NoiseTexColor, uvc).r;

				float3 color = lerp(_Color.rgb,_NoiseTintColor.rgb, colorNoiseLerp * _ColorNoiseIntensity) * clamp(intensity*_ColorMidStrength,_ColorAlwaysAbove1 ? 1 : 0, 50000);

				//Alpha
				float2 uva = float2(uv.x* _ScaleAlpha - (_Time.x * _SpeedXAlpha), uv.y * _ScaleAlpha - (_Time.x * _SpeedYAlpha));
				float NoiseAlpha = _DefaultNoiseAlpha;
				if(_UseNoiseAlpha)NoiseAlpha = SAMPLE_TEXTURE2D(_NoiseTexAlpha, sampler_NoiseTexAlpha, uva).r * _MultiplierAlpha;
				float alpha = _Color.a * (intensity) * NoiseAlpha;

				return float4(color, saturate(alpha)) + SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);

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
