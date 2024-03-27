Shader "Unlit/RaymarchSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            #define MAX_STEPS 100
            #define MAX_DIST 100
            #define SURF_DIST 1e-3

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv : TEXCOORD0;              
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 ro : TEXCOORD1;
                float3 hitPos : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1));
                o.hitPos = v.vertex;
                return o;
            }

            float smoothMax(float a, float b, float k) {
                return log(exp(k * a) + exp(k*b)) / k;
            }

            float smoothMin(float a, float b, float k) {
                return -smoothMax(-a, -b, k);
            }

            float boxSDF(float3 p) {
                return length(max(abs(p)- float3(0.1, 0.5, 0.5), 0));
            }

            float sphereSDF(float3 p) {
                return length(p) - 0.5;
            }
            
            float GetDist(float3 p) {
                float d = smoothMin(boxSDF(p), sphereSDF(p), 10);
                return d;
            }

            float Raymarch(float3 ro, float3 rd) {
                float dO = 0;
                float dS;
                for (int i = 0; i < MAX_STEPS; i++) {
                    float3 p = ro + dO * rd;
                    dS = GetDist(p);
                    dO += dS;
                    if (dS < SURF_DIST || dO > MAX_DIST) break;
                }

                return dO;
            }

            float3 GetNormal(float3 p) {
                float2 e = float2(1e-2, 0);
                float3 n = GetDist(p) - float3(GetDist(p - e.xyy),GetDist(p - e.yxy),GetDist(p - e.yyx));

                return normalize(n);
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5;
                float3 ro = i.ro; //float3(0,0,-3);
                float3 rd = normalize(i.hitPos - ro);
                half4 col = 0;
                float d = Raymarch(ro, rd);
                
                if (d < MAX_DIST) {
                    //col.r = 1;
                    float3 p = ro + rd * d;
                    float3 n = GetNormal(p);
                    col.rgb = n;
                } else {
                    discard;
                }
                //col.rgb = rd;
            
                return col;
            }
            ENDHLSL
        }
    }
}