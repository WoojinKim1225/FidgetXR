using UnityEngine;
using UnityEngine.Events;

public class Point : FingerInteractable
{
    public override float DistanceFunction(Transform from){
        return Vector3.Distance(from.position, transform.position);
    }
}