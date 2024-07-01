using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Arc : FingerInteractable
{
    public Vector3 centerPositionOS = Vector3.zero;
    public Vector3 axisOS = Vector3.up;
    public Vector3 axis2OS = Vector3.forward;
    public float startAngle = -90f;
    public float endAngle = 90f;
    public float radius = 0.01f;

    public LineRenderer arcVisual;
    public int arcSegmentCount = 3;
    public MeshRenderer valueVisual;

    public Vector3 positionWS;
    public Vector3 positionWSUnclamped;
    public Vector3 positionOS;
    public Vector3 positionOSUnclamped;
    public float positionAngle;
    public float positionAngleUnclamped;
    public float position01;
    public float position01Unclamped;

    public Float valueAngle = new Float(0);

    public override float DistanceFunction(Transform from){
        axisOS = Vector3.Normalize(axisOS);
        axis2OS = Vector3.Normalize(Vector3.ProjectOnPlane(axis2OS, axisOS));
        Vector3 directionWS = Vector3.ProjectOnPlane(from.position - transform.TransformPoint(centerPositionOS), transform.TransformDirection(axisOS)).normalized;
        positionWSUnclamped = transform.TransformPoint(centerPositionOS) + directionWS * radius;
        positionAngleUnclamped = Vector3.SignedAngle(transform.TransformDirection(axis2OS), directionWS, transform.TransformDirection(axisOS));
        position01Unclamped = Mathf.InverseLerp(startAngle, endAngle, positionAngleUnclamped);
        positionOSUnclamped = transform.InverseTransformPoint(positionWSUnclamped);

        position01 = Mathf.Clamp01(position01Unclamped);
        positionAngle = Mathf.Lerp(startAngle, endAngle, position01);
        positionOS = Quaternion.AngleAxis(positionAngle, axisOS) * axis2OS * radius;
        positionWS = transform.TransformPoint(positionOS);

        return Vector3.Distance(from.position, positionWS);
    }

    void Awake()
    {
        arcVisual = GetComponentInChildren<LineRenderer>();    
    }

    public void Update()
    {
        valueAngle.OnUpdate();
        arcVisual.positionCount = arcSegmentCount + 1;
        arcVisual.transform.forward = -transform.TransformDirection(axisOS);
        for (int i = 0; i < arcVisual.positionCount; i++) {
            float angle = Mathf.Lerp(startAngle, endAngle, i * 1.0f / arcSegmentCount);
            Vector3 posOS = Quaternion.AngleAxis(angle, axisOS.normalized) * axis2OS.normalized * radius;
            arcVisual.SetPosition(i, transform.TransformPoint(posOS));
        }
        valueVisual.sharedMaterial.SetFloat("_From", startAngle);
        valueVisual.sharedMaterial.SetFloat("_To", valueAngle.value);
    }
}
