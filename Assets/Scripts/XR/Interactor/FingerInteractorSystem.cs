using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial class FingerInteractorTransformUpdate : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach(
            (FingerReference reference, 
            FingerInteractableReference interactableReference, 
            ref FingerData data) => 
            { 
                data.TipTransformWS = new RigidTransform(
                    reference.Tip.rotation, 
                    reference.Tip.position);
                data.BaseTransformWS = new RigidTransform(
                    reference.Base.rotation, 
                    reference.Base.position);
                
                data.ThumbTipPositionWS = reference.ThumbTip.position;
                data.IsThumb = reference.ThumbTip == reference.Tip;

                if (interactableReference.InteractableObject != null) {
                    data.InteractablePositionWS.xyz = interactableReference.InteractableObject.transform.position;
                    data.InteractablePositionWS.w = 1f;
                } else {
                    data.InteractablePositionWS = 0f;
                }
            }
        ).WithoutBurst().Run();
    }
}

[UpdateAfter(typeof(FingerInteractorTransformUpdate))]
public partial struct FingerInteractorDataUpdate : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new FingerInteractorDataUpdateJob();
        job.ScheduleParallel();
    }
}

public partial struct FingerInteractorDataUpdateJob : IJobEntity
{
    public void Execute(ref FingerData data) {
        data.Curl = GetCurl(data.BaseTransformWS.rot, data.TipTransformWS.rot);
        data.Pinch = GetPinch(data.TipTransformWS.pos, data.ThumbTipPositionWS);
        data.Touch = GetTouch(data.TipTransformWS.pos, data.InteractablePositionWS);
        data.Tap = GetTap(data.ThumbTipPositionWS, data.BaseTransformWS.pos);
    }

    private float GetCurl(quaternion a, quaternion b) {
        return math.saturate(1 - math.dot(math.mul(a, math.forward()), math.mul(b, math.forward())));
    }
    private float GetPinch(float3 a, float3 b) {
        return math.saturate(1 - (math.distance(a, b) / 0.05f));
    }
    private float GetTouch(float3 a, float4 b) {
        if (b.w == 0) return -1;
        else return math.saturate(1 - (math.distance(a, b.xyz) / 0.05f));
    }
    private float GetTap(float3 a, float3 b) {
        return math.saturate(1 - (math.distance(a, b) / 0.05f));
    }
}