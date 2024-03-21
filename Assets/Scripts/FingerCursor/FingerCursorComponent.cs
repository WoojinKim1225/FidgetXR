using Unity.Entities;
using Unity.Mathematics;

public struct FingerData : IComponentData {
    public float3 Axis;
    public float Angle;
}

public struct FingerPointData : IComponentData {
    public Entity pointEntity;
    public float4x4 Transform;
}