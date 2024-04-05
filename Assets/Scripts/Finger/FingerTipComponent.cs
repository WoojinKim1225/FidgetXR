using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Finger
{
    public struct ConnectedFingerTransform : IComponentData
    {
        public RigidTransform Tip, Root, ThumbTip, Palm;
        public float3 frontAxis, upAxis;
    }

    public struct ConnectedThumbEntity : IComponentData
    {
        public Entity Thumb;
    }
    public struct ConnectedPalmEntity : IComponentData
    {
        public Entity Palm;
    }

    public struct HandData : IComponentData
    {
        public int HandNumber;
    }

    public struct FingerAction : IComponentData
    {
        public float Curl, Bend, Pinch;
        public float CurlEnableMargin, CurlDisableMargin;
        public float BendEnableMargin, BendDisableMargin;
        public float PinchEnableMargin, PinchDisableMargin;
    }

    public delegate void MyDelegate();
    public class FingerDelegate : IComponentData
    {
        public MyDelegate curl;
        public MyDelegate pinch;
    }
    public class ConnectedFingerReference : IComponentData
    {
        public GameObject TipGameObject, RootGameObject, ThumbTipGameObject, PalmGameObject;
    }
}