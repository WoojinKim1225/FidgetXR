Shader "Custom/HandShader"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 3
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Pass", Int) = 3
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline"}

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        /*
        Stencil
        {
            Ref [_StencilID]
            Comp [_StencilComp]
            Pass [_StencilOp]
            Fail Keep

        }
        */

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS: POSITION;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float4 positionOS : TEXCOORD0;
            };

            Varyings vert(Attributes i) {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
                o.positionOS = i.positionOS;
                return o;
            };

            half4 frag(Varyings vert) : SV_TARGET {
                //return vert.positionOS.z;
                return half4(0.5, 0, 0, .5);
            }
            ENDHLSL
        }
    }
}
