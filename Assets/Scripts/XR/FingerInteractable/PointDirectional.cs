using UnityEngine;

public class PointDirectional : FingerInteractable
{
    public Vector3 axisOS;

    public override float DistanceFunction(Transform from){
        return Vector3.Dot(transform.position - from.position, transform.TransformDirection(axisOS.normalized));
    }
}