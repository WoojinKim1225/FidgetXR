using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Finger
{
    public partial struct FingerTipMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, trs, reference) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<ConnectedFingerTransform>, ConnectedFingerReference>())
            {
                transform.ValueRW.Position = reference.TipGameObject.transform.position;
                transform.ValueRW.Rotation = reference.TipGameObject.transform.rotation;

                trs.ValueRW.Tip = new RigidTransform(reference.TipGameObject.transform.localToWorldMatrix);
                trs.ValueRW.Root = new RigidTransform(reference.RootGameObject.transform.localToWorldMatrix);
                trs.ValueRW.ThumbTip = new RigidTransform(reference.ThumbTipGameObject.transform.localToWorldMatrix);
                trs.ValueRW.Palm = new RigidTransform(reference.PalmGameObject.transform.localToWorldMatrix);

                UnityEngine.Debug.DrawLine(float3.zero, transform.ValueRW.Position);
            }
        }
    }

    [BurstCompile]
    public partial struct FingerActionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (trs, action) in SystemAPI.Query<RefRO<ConnectedFingerTransform>, RefRW<FingerAction>>())
            {
                float3 a = math.mul(trs.ValueRO.Tip.rot, trs.ValueRO.frontAxis);
                float3 b = math.mul(trs.ValueRO.Root.rot, trs.ValueRO.frontAxis);
                float3 c = math.mul(trs.ValueRO.Palm.rot, trs.ValueRO.upAxis);
                action.ValueRW.Curl = math.dot(a, b);
                action.ValueRW.Bend = math.dot(b, c);
                action.ValueRW.Pinch = math.distance(trs.ValueRO.Tip.pos, trs.ValueRO.ThumbTip.pos);
            }
        }
    }

    public partial struct FingerDelegateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (action, d) in SystemAPI.Query<RefRO<FingerAction>, FingerDelegate>())
            {
                
            }
        }
    }
}