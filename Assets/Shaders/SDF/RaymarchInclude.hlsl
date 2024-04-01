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


float GetDist(float3 p, SDFVars var) {
    //float d = smoothMin(sphereSDF(p, var.p1.xyz, 0.03), sphereSDF(p, var.p2.xyz, 0.03), 100);
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
    return d;
}

float3 GetNormal(float3 p, SDFVars var) {
    float2 e = float2(1e-2, 0);
    float3 n = GetDist(p, var) - float3(GetDist(p + e.xyy, var),GetDist(p + e.yxy, var),GetDist(p + e.yyx, var));

    return normalize(n);
}

void Raymarch_half(half3 ro, half3 rd, half3 rdMid, half depth, half3 p_L, half3 p_R, half r1_L, half r1_R, half3 d_L, half3 d_R, half t_L, half t_R, out half3 p, out half3 n, out half a) {
    half dO = 0;
    half dS;

    SDFVars var;
    var.p_L = p_L;
    var.p_R = p_R;
    var.d_L = d_L;
    var.d_R = d_R;
    var.r1_L = r1_L;
    var.r1_R = r1_R;
    var.t_L = t_L;
    var.t_R = t_R;

    for (int i = 0; i < MAX_STEPS; i++) {
        half3 p = ro + dO * rd;
        dS = GetDist(p, var);
        dO += dS;
        if (dS < SURF_DIST || dO > MAX_DIST) {
            break;
        }
    }
    if (dO < MAX_DIST && dO * dot(rd, rdMid) < depth) {
        p.xyz = ro + rd * dO;
        a = 1;
        n = GetNormal(p, var);
    } else {
        p = half3(0,0,0);
        a = 0;
        n = half3(0,0,0);
    }
    //return dO;
}


#endif