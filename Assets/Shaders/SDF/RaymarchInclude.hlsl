#ifndef RAYMARCH_INCLUDED
#define RAYMARCH_INCLUDED

#include "Assets/Shaders/SDF/RaymarchUtils.hlsl"

#define MAX_STEPS 100
#define MAX_DIST 100
#define SURF_DIST 1e-3

struct SDFVars {
    float3 p_L;
    float3 d_L;
    float r1_L;
    float3 p_R;
    float3 d_R;
    float r1_R;
    float t_L;
    float t_R;
};


float4 GetDist(float3 p) {
    //float d = smoothMin(sphereSDF(p, var.p1.xyz, 0.03), sphereSDF(p, var.p2.xyz, 0.03), 100);
    /*
    float d = min(
        lerp(
            sdRoundCone(p, var.p_L, var.p_L + var.d_L, var.r1_L, 0.005), 
            sdSphere(p, var.p_L, var.r1_L), 
            var.t_L
        ), 
        lerp(
            sdRoundCone(p, var.p_R, var.p_R + var.d_R, var.r1_R, 0.005),
            sdSphere(p, var.p_R, var.r1_R),
            var.t_R
        )        
    );
    */
    //float4 b = float4(0,0,1,sdSphere(p, float3(_SinTime.w,0,0), 0.1));
    //float4 c = float4(0,1,0,sdSphere(p, float3(-_SinTime.w,0,0), 0.2));
    /*
    float4 b = float4(.5, .5, 1, lerp(
        sdRoundCone(p, var.p_L, var.p_L + var.d_L, var.r1_L, 0.005), 
        sdSphere(p, var.p_L, var.r1_L), 
        var.t_L)
    );

    float4 c = float4(1, .5, .5, lerp(
        sdRoundCone(p, var.p_R, var.p_R + var.d_R, var.r1_R, 0.005),
        sdSphere(p, var.p_R, var.r1_R),
        var.t_R) 
    );
    

    float4 d = sdSmoothUnion(b,c, 0.5);
    return d;
    */
    return float4(1, 1, 1, sdSphere(p, float3(0,1.2,1), 0.1));
}

float3 GetNormal(float3 p) {
    float2 e = float2(1e-2, 0);
    float3 n = GetDist(p).w - float3(GetDist(p + e.xyy).w, GetDist(p + e.yxy).w, GetDist(p + e.yyx).w);

    return normalize(n);
}

void Raymarch_half(float3 ro, float3 rd, float3 camForward, float depth, out half3 p, out half3 n, out half3 c, out half a) {

    
    half dO = 0;
    half4 dS;
    for (int i = 0; i < MAX_STEPS; i++) {
        if (dO > MAX_DIST) {
            break;
        }
        half3 p = ro + dO * rd;
        dS = GetDist(p);
        if (dS.w < SURF_DIST) {
            c = dS.xyz;
            break;
        }
        dO += dS.w;
    }
    if (dO < MAX_DIST && dO * dot(rd, camForward) < depth) {
        p.xyz = ro + rd * dO;
        a = 1;
        n = GetNormal(p);
    } else {
        p = half3(0,0,0);
        a = 0;
        c = half3(0,0,0);
        n = half3(0,0,0);
    }
}


#endif