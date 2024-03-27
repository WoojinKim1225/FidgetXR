Shader "ColorBlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (vert),
            // input structure (Attributes) and o strucutre (v2f)
            // #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GlobalSamplers.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            #define MAX_STEPS 100
            #define MAX_DIST 100
            #define SURF_DIST 1e-3

            TEXTURE2D_X(_BlitTexture);
            TEXTURECUBE(_BlitCubeTexture);

            uniform float4 _BlitScaleBias;
            uniform float4 _BlitScaleBiasRt;
            uniform float _BlitMipLevel;
            uniform float2 _BlitTextureSize;
            uniform uint _BlitPaddingSize;
            uniform int _BlitTexArraySlice;
            uniform float4 _BlitDecodeInstructions;

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _Intensity;

            struct Attributes
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv   : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4x4 _CameraToWorld;
            float4x4 _ProjectionInverse;

            v2f vert(Attributes input)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float4 pos = GetFullScreenTriangleVertexPosition(input.vertexID);
                float2 uv  = GetFullScreenTriangleTexCoord(input.vertexID);

                o.positionCS = pos;
                o.uv = uv * _BlitScaleBias.xy + _BlitScaleBias.zw;

                float4 worldPos = mul(_CameraToWorld, mul(_ProjectionInverse, pos));
                // Calculate view direction in world space
                o.viewDir = normalize(worldPos.xyz - _WorldSpaceCameraPos);

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, i.uv);
                //color.xy = i.uv;
                //color.zw = 0;
                color = float4(i.viewDir, 1);
                return color; // * float4(0, _Intensity, 0, 1);
            }
            ENDHLSL
        }
    }
}