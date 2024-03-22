using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.XR.Oculus;


[BurstCompile]
public partial struct FingerCursorSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //GetCurrentStartBoneID()
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}
