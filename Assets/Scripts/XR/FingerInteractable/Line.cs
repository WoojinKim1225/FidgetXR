using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Line : FingerInteractable
{
    public Vector3 startPositionOS = Vector3.left;
    public Vector3 endPositionOS = Vector3.right;

    private Vector3 startPosWS => transform.rotation * startPositionOS + transform.position;
    private Vector3 endPosWS => transform.rotation * endPositionOS + transform.position;

    public LineRenderer lineVisual;
    public Transform valueVisual;

    public Vector3 positionWS;
    public Vector3 positionWSUnclamped;
    public Vector3 positionOS;
    public Vector3 positionOSUnclamped;
    public float position01;
    public float position01Unclamped;

    public float value01;

    public override float DistanceFunction(Transform from){
        positionWSUnclamped = startPosWS + Vector3.Dot(from.position - startPosWS, Vector3.Normalize(endPosWS - startPosWS)) * Vector3.Normalize(endPosWS - startPosWS);
        position01Unclamped = (positionWS - startPosWS).magnitude / (endPosWS - startPosWS).magnitude;
        positionOSUnclamped = Vector3.LerpUnclamped(startPositionOS, endPositionOS, position01Unclamped);
        
        position01 = Mathf.Clamp01(position01Unclamped);
        positionOS = Vector3.Lerp(startPositionOS, endPositionOS, position01);
        positionWS = Vector3.Lerp(startPosWS, endPosWS, position01);

        return Vector3.ProjectOnPlane(from.position - startPosWS, Vector3.Normalize(endPosWS - startPosWS)).magnitude;
    }

    public void SetValue() {
        value01 = position01;
        valueVisual.localPosition = positionOS;
    }

    void Awake()
    {
        lineVisual = GetComponentInChildren<LineRenderer>(); 
    }
    public void Update()
    {
        lineVisual.positionCount = 2;
        lineVisual.SetPosition(0, startPosWS);
        lineVisual.SetPosition(1, endPosWS);
    }
}
