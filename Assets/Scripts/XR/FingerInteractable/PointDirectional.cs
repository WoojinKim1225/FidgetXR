using UnityEngine;

public class PointDirectional : FingerInteractable
{
    public override float DistanceFunction(Transform from){
        return Vector3.Dot(transform.position - from.position, transform.forward);
    }
}