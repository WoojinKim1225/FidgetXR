#ifndef RAYMARCH_INCLUDED
#define RAYMARCH_INCLUDED

#include "Assets/Shaders/SDF/RaymarchUtils.hlsl"

#define MAX_STEPS 100
#define MAX_DIST 100
#define SURF_DIST 1e-3

struct SDFVars {
    float3 position;
    float3 direction;
    float radius;
};


float4 GetDist(float3 p, SDFVars s) {
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
    //return float4(1, 1, 1, sdSphere(p, s.position, s.radius));
    return float4(1,1,1, sdRoundCone(p, s.position, s.position + s.direction, s.radius, 0.002));
}

float3 GetNormal(float3 p, SDFVars s) {
    float2 e = float2(1e-2, 0);
    float3 n = GetDist(p, s).w - float3(GetDist(p + e.xyy, s).w, GetDist(p + e.yxy, s).w, GetDist(p + e.yyx, s).w);

    return normalize(n);
}

void Raymarch_float(float3 ro, float3 rd, float3 camForward, float depth, float3 position, float3 direction, float radius, out float3 p, out float3 n, out float3 c, out float a) {
    SDFVars sVars;
    sVars.position = position;
    sVars.direction = direction;
    sVars.radius = radius;

    float dO = 0;
    float4 dS;
    for (int i = 0; i < MAX_STEPS; i++) {
        if (dO > MAX_DIST) {
            break;
        }
        float3 p = ro + dO * rd;
        dS = GetDist(p, sVars);
        if (dS.w < SURF_DIST) {
            c = dS.xyz;
            break;
        }
        dO += dS.w;
    }
    if (dO < MAX_DIST && dO * dot(rd, camForward) < depth) {
        p.xyz = ro + rd * dO;
        a = 1;
        n = GetNormal(p, sVars);
    } else {
        p = 0;
        a = 0;
        c = 0;
        n = 0;
    }
}

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

void GetShadows_float(float3 pos, out float a) {
#ifdef SHADERGRAPH_PREVIEW
    a = 0.5;
#else
#if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(pos);
    float4 shadowCoord = ComputeScreenPos(clipPos);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(pos);
#endif
    Light light = GetMainLight(shadowCoord);
    a = light.shadowAttenuation;
#endif
}


#endif