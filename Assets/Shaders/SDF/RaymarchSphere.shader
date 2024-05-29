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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
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
            //uniform sampler2D _CameraDepthTexture;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.ro = _WorldSpaceCameraPos;
                o.hitPos = v.vertex;//mul(unity_ObjectToWorld, v.vertex.xyz);
                return o;
            }
            /*
            float smoothMax(float a, float b, float k) {
                return log(exp(k * a) + exp(k * b)) / k;
            }

            float smoothMin(float a, float b, float k) {
                return -smoothMax(-a, -b, k);
            }

            float boxSDF(float3 p) {
                return length(max(abs(p)- float3(0.1, 0.5, 0.5), 0));
            }

            float sphereSDF(float3 p, float3 center, float radius) {
                return length(p - center) - radius;
            }
            */
            
            float GetDist(float3 p) {
                float d = length(p) - 0.5;
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

            float4 frag(v2f i) : SV_Target
            {
                //Light mainLight = GetMainLight();
                //float3 lDirOS = mul(unity_WorldToObject, mainLight.direction);
                float2 uv = i.uv - 0.5;
                float3 ro = i.ro;
                float3 rd = normalize(i.hitPos - ro);
                //float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, i.screenPos.xy).r, _ZBufferParams);
                float4 col = 0;
                float d = Raymarch(ro, rd);
                //col.x = depth;
                //return col;

                if (d < MAX_DIST) {
                    float3 p = ro + rd * d;
                    float3 n = GetNormal(p);
                    //col = saturate(dot(lDirOS, n));
                    col.xyz = n;
                } else {
                    discard;
                }
                
                return col;
            }
            ENDHLSL
        }
    }
}