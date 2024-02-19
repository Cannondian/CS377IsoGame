Shader "Aura/EnlargedObjects"
{
   HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		//#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
        #pragma multi_compile _Tex1 _Tex2 _Tex3 _Tex4 _Tex5

        #ifdef _Tex1
		#define _SceneMask _SceneMask1
		#define sampler_SceneMask sampler_SceneMask1

		#elif _Tex2
		#define _SceneMask _SceneMask2
		#define sampler_SceneMask sampler_SceneMask2

		#elif _Tex3
		#define _SceneMask _SceneMask3
		#define sampler_SceneMask sampler_SceneMask3

        #elif _Tex4
		#define _SceneMask _SceneMask4
		#define sampler_SceneMask sampler_SceneMask4

         #elif _Tex5
		#define _SceneMask _SceneMask5
		#define sampler_SceneMask sampler_SceneMask5
		#endif

		TEXTURE2D(_SceneMask);
		SAMPLER(sampler_SceneMask);

        float _UseDepthMask;
        float _UseBehindWallColor;

		struct Attributes
            {
                float4 position     : POSITION;
                float2 texcoord     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
                float4 screenPos    : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.position.xyz);

                output.uv = input.texcoord;
                output.positionCS = positionInputs.positionCS;
                output.screenPos = positionInputs.positionNDC;
                return output;
            }

		 half4 frag(Varyings input) : SV_TARGET
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                if(_UseDepthMask)
                {
                    float4 mask = SAMPLE_TEXTURE2D(_SceneMask, sampler_SceneMask, input.screenPos.xy / input.screenPos.w);

                    //if(_UseBehindWallColor)
                    //{
                    //    if (mask.r >= input.positionCS.z)
			        //    {
			    	//        return float4(1,1,0,1);
			        //    }
                    //
                    //   return float4(0,1,0,1);
                    //}

                    if (mask.r > input.positionCS.z)
			        {
			    	    return float4(1,0,0,1);
			        }

                }
                
                return float4(0,1,0,1);
             }

	ENDHLSL

	SubShader
	{
		Tags { "RenderPipeline" = "UniversalPipeline" }

		Cull Off
		//ZWrite Off
		ZTest LEqual
		Lighting Off

		Pass
		{
			Name "ObjetsRender"

			 HLSLPROGRAM
	        #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
		}
	}
}
