// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Unlit/BlockShader"
{
    Properties
    {
        _TextureArray ("Texture", 2DArray) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			//
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup
			#pragma instancing_options procedural:vertInstancingSetup
			//#pragma exclude_renderers gles
			//#define UNITY_PARTICLE_INSTANCE_DATA MyParticleInstanceData
			//#define UNITY_PARTICLE_INSTANCE_DATA_NO_ANIM_FRAME

            #include "UnityCG.cginc"
			#include "UnityStandardParticleInstancing.cginc"
			//#include "UnityInstancing.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float1 id : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float1 id : TEXCOORD1;
            };

			UNITY_DECLARE_TEX2DARRAY(_TextureArray);
            float4 _TextureArray_ST;

            v2f vert (appdata v)
            {
				UNITY_SETUP_INSTANCE_ID(v);
				v2f o;

#if defined(UNITY_PARTICLE_INSTANCING_ENABLED)
				float2 uv_modded = float2(
					v.uv.x / 8.0f + float(fmod(unity_InstanceID, 8)) / 8.0f,
					v.uv.y / 8.0f + float(fmod(unity_InstanceID * 100, 8)) / 8.0f
					);
				o.uv = TRANSFORM_TEX(uv_modded, _TextureArray);
#else
				o.uv = TRANSFORM_TEX(v.uv, _TextureArray);
#endif
                
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.id = v.id;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = UNITY_SAMPLE_TEX2DARRAY(_TextureArray, float3(i.uv, i.id));

                return col;
            }
            ENDCG
        }
    }
}
