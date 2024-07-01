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
    public Transform valueVisual;

    public Float valueAngle = new Float(0);

    public override float DistanceFunction(Transform from){
        return 0;
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
    }
}
