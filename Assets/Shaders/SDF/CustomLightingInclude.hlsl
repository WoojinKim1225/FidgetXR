#ifndef CUSTOMLIGHTING_INCLUDED
#define CUSTOMLIGHTING_INCLUDED

void GetMainLightColor_half(out half3 color) {
    #if SHADERGRAPH_PREVIEW
        color = half3(1,1,1);
        
    #else
        Light l = GetMainLight();
        color = l.color.xyz;
    #endif
}

void Lambert_half(half4 albedo, half3 normal, half3 lightDir, half3 lightColor, out half4 color)
{
    half diffuse = saturate(dot(normal, lightDir));
    color.xyz = albedo.xyz * diffuse * lightColor;
    color.w = albedo.w;
}

void SmoothLambert_half(half4 albedo, half3 normal, half3 lightDir, half3 lightColor, out half4 color)
{
    half diffuse = saturate(dot(normal, lightDir) * 0.5 + 0.5) * saturate(dot(normal, lightDir) * 0.5 + 0.5);
    color.xyz = albedo.xyz * diffuse * lightColor;
    color.w = albedo.w;
}   

void BlinnPhong_half(half4 albedo, half3 normal, half3 lightDir, half3 lightColor, half3 viewDir, half smoothness, half rimThreshold, out half4 color)
{
    half diffuse = saturate(dot(normal, lightDir));
    half3 h = normalize(lightDir - viewDir);

    half nh = saturate(dot(normal, h));
    half specular = pow(nh, exp2(10 * smoothness + 1));
    specular *= diffuse * smoothness;

    half rim = 1 - dot(viewDir, normal);
    rim *= pow(diffuse, rimThreshold);
    
    color.xyz = albedo.xyz * (diffuse + max(specular, rim)) * lightColor;
    color.w = albedo.w;
}

void SmoothBlinnPhong_half(half4 albedo, half3 normal, half3 lightDir, half3 lightColor, half3 viewDir, half smoothness, half rimThreshold, out half4 color)
{
    half diffuse = saturate(dot(normal, lightDir) * 0.5 + 0.5) * saturate(dot(normal, lightDir) * 0.5 + 0.5);
    half3 h = normalize(lightDir - viewDir);

    half nh = saturate(dot(normal, h));
    half specular = pow(nh, exp2(10 * smoothness + 1));
    specular *= diffuse * smoothness;

    half rim = 1 - dot(viewDir, normal);
    rim *= pow(diffuse, rimThreshold);

    color.xyz = albedo.xyz * (diffuse + max(specular, rim)) * lightColor;
    color.w = albedo.w;
}

#endif