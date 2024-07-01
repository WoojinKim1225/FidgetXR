#ifndef SSScatter_INCLUDED
#define SSScatter_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

float3 FastSubSurface_float(float3 lightDir, float3 lightColor, float3 normalWS, float3 viewDir, float3 subsurfaceColor, float subsurfacePower, float subsurfaceScale, out float3 subsurfaceLighting) {
    float NdotL = dot(normalWS, lightDir);
    float VdotL = dot(viewDir, -lightDir);

    float subsurface = pow(saturate(VdotL), subsurfacePower) * subsurfaceScale;

    subsurfaceLighting = subsurface * lightColor * subsurfaceColor;
}
#endif