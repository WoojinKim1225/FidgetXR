using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshInteractable : MonoBehaviour
{
    [SerializeField] private MeshVisualizer _meshManager;
    private Mesh _mesh;

    public Vector2 size;

    public enum EMeshType {
        Flat, Curved, Spherical
    }
    public EMeshType meshType = EMeshType.Flat;

    [SerializeField] private float curvature = 0.25f;
    [SerializeField] private float resolution = 0.01f;

    public Transform pointer;

    public Vector3 pointerOrigin => pointer.position;
    public Vector3 pointerDirection => pointer.forward;
    
    /*
    Vector3 GetHitPointOS(Vector3 origin, Vector3 direction) {
        var invalidPosition = new Vector3(float.NaN, float.NaN, float.NaN);
        Vector3 originOS = transform.InverseTransformPoint(origin);
        Vector3 directionOS = transform.InverseTransformDirection(direction).normalized;
        float radius = 1f / curvature;

        switch (meshType) {
            case EMeshType.Flat:
                if (originOS.z * directionOS.z < 0)
                    return originOS - directionOS * originOS.z / directionOS.z;
                else
                    return invalidPosition;
            case EMeshType.Curved:
                Vector3 horizontalDirOS = Vector3.Normalize(directionOS - Vector3.up * directionOS.y);
                Vector3 horizontalOriginOS = originOS - Vector3.up * originOS.y;
                float tan = directionOS.y/ Mathf.Sqrt(directionOS.x * directionOS.x + directionOS.z * directionOS.z);
                Vector3 h = Vector3.ProjectOnPlane(horizontalOriginOS - radius * Vector3.back, horizontalDirOS);
                Vector3 horizontalPosOS = radius * Vector3.back + h + horizontalDirOS * Mathf.Sqrt(1 - Vector3.SqrMagnitude(h) * curvature * curvature) * radius;
                return horizontalPosOS + Vector3.up * (originOS.y + Vector3.Distance(horizontalPosOS, horizontalOriginOS) * tan);
            case EMeshType.Spherical:
                break;
        }
        return invalidPosition;
    }
    */
    

    private void Update() {
        _meshManager.size = size;
        _meshManager.curvature = meshType == EMeshType.Flat ? 0f : curvature;
        _meshManager.resolution = resolution;

    }
    
    
    
}
