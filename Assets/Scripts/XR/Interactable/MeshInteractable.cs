using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshInteractable : MonoBehaviour
{
    public PlaneMeshGenerator meshGenerator;
    private Mesh mesh;



    public Vector2 size;

    public enum EMeshType {
        Flat, Curved, Spherical
    }
    public EMeshType meshType = EMeshType.Flat;

    public float curvature = 0.25f;
    public float resolution = 0.01f;

    public Transform pointer;

    
    
    Vector3? GetHitPointOS(Vector3 origin, Vector3 direction) {
        Vector3 originOS = transform.InverseTransformPoint(origin);
        Vector3 directionOS = transform.InverseTransformDirection(direction).normalized;
        switch (meshType) {
            case EMeshType.Flat:
                if (originOS.z * directionOS.z < 0)
                    return originOS - directionOS * originOS.z / directionOS.z;
                else
                    return null;
            case EMeshType.Curved:
                Vector3 h = Vector3.ProjectOnPlane(originOS + (1f / curvature) * Vector3.up, directionOS);
                return (1f / curvature) * Vector3.down + h + directionOS * Mathf.Sqrt(1 - Vector3.SqrMagnitude(h) * curvature * curvature) / curvature;
            case EMeshType.Spherical:
                break;
        }
        return Vector3.zero;
    }
    
    private void Start() {
        
    }

    private void Update() {
        meshGenerator.size = size;
        meshGenerator.curvature = meshType == EMeshType.Flat ? 0f : curvature;
        meshGenerator.resolution = resolution;

        Vector3? hitPosOS = GetHitPointOS(pointer.position, pointer.forward);
        if (hitPosOS.HasValue)
            Debug.DrawLine(pointer.position, transform.TransformPoint(hitPosOS.Value), Color.cyan);
    }
    
    
    
}
