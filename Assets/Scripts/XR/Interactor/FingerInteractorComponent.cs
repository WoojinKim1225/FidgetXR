using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct FingerData : IComponentData, IEnableableComponent
{
    public RigidTransform BaseTransformWS, TipTransformWS;
    public float3 ThumbTipPositionWS;
    public bool IsThumb;
    public float4 InteractablePositionWS;

    public float Curl;
    public float Pinch;
    public float Touch;
    public float Tap;
}

public class FingerReference : IComponentData {
    public Transform Base, Tip, ThumbTip;
}


public class FingerInteractableReference : IComponentData {
    public GameObject InteractableObject;
}
