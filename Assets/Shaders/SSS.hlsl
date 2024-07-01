#ifndef SSScatter_INCLUDED
#define SSScatter_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

float3 FastSubSurface_float(float3 normalWS, float3 viewDir, float3 subsurfaceColor, float subsurfacePower, float subsurfaceScale, out float3 subsurfaceLighting) {
    float NdotL = dot(normalWS, GetMainLight().direction);
    float VdotL = dot(viewDir, -GetMainLight().direction);

    float subsurface = pow(saturate(VdotL), subsurfacePower) * subsurfaceScale;

    subsurfaceLighting = subsurface * GetMainLight().color, subsurfaceColor;
}
#endif