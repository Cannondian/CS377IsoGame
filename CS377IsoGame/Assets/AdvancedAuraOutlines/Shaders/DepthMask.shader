Shader "Aura/DepthMask"
{
    HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

		half4 FragmentSimple(Varyings input) : SV_Target
		{
			return input.positionCS.z;
		}

	ENDHLSL

	SubShader
	{
		Tags { "RenderPipeline" = "UniversalPipeline" }

		//Cull Off
		ZTest LEqual
		Lighting Off

		Pass
		{
			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma vertex Vert
			#pragma fragment FragmentSimple

			ENDHLSL
		}
	}
}
