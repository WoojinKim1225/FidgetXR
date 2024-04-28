using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial class FingerInteractorTransformUpdate : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach(
            (FingerReference reference,ref FingerData data) => { 
                data.TipTransformWS = new RigidTransform(reference.Tip.rotation, reference.Tip.position);
                data.BaseTransformWS = new RigidTransform(reference.Base.rotation, reference.Base.position);
                data.ThumbTipPositionWS = reference.ThumbTip.position;
                data.IsThumb = reference.ThumbTip == reference.Tip;
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
        data.Pinch = math.distance(data.TipTransformWS.pos, data.ThumbTipPositionWS);
    }

    private float GetCurl (quaternion a, quaternion b) {
        return math.dot(math.mul(a, math.forward()), math.mul(b, math.forward()));
    }
}