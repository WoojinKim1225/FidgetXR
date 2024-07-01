using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Plane : FingerInteractable
{
    [SerializeField] Vector3 _centerPositionOS = Vector3.zero;
    [SerializeField] Vector3 _axisRightOS = Vector3.right * 0.1f;
    private Vector3 _axisRightOSBefore;
    [SerializeField] Vector3 _axisUpOS = Vector3.up * 0.1f;
    private Vector3 _axisUpOSBefore;

    public Vector2 size => Vector2.right * _axisRightOS.magnitude + Vector2.up * _axisUpOS.magnitude;

    public PlaneMesh planeVisual;

    public Vector3 positionWS;
    public Vector3 positionWSUnclamped;
    public Vector3 positionOS;
    public Vector3 positionOSUnclamped;
    public Vector2 positionUV;
    public Vector2 positionUVUnclamped;
    

    public override float DistanceFunction(Transform from){
        Vector3 planeNormalWS = -transform.TransformDirection(Vector3.Cross(_axisRightOS, _axisUpOS)).normalized;
        Vector3 planeCenterWS = transform.TransformPoint(_centerPositionOS);
        bool intersects = RayIntersectsPlane(planeCenterWS, planeNormalWS, from.position, from.forward, out Vector3 intersectionPointWS);
        positionWSUnclamped = intersectionPointWS;
        positionOSUnclamped = planeVisual.transform.InverseTransformPoint(intersectionPointWS);
    
        positionUVUnclamped = positionOSUnclamped / size + 0.5f * Vector2.one;
        positionUV = new Vector2(Mathf.Clamp01(positionUVUnclamped.x), Mathf.Clamp01(positionUVUnclamped.y));
        positionOS = (positionUV - 0.5f * Vector2.one) * size;
        positionWS = planeVisual.transform.TransformPoint(positionOS);

        Vector3 v = planeVisual.transform.InverseTransformPoint(from.position);
        v.x = Mathf.Clamp(v.x, -size.x * 0.5f, size.x * 0.5f);
        v.y = Mathf.Clamp(v.y, -size.y * 0.5f, size.y * 0.5f);
        v.z = 0;

        return Vector3.Distance(planeVisual.transform.TransformPoint(v), from.position);
    }

    public void Update()
    {
        _axisUpOS = Vector3.ProjectOnPlane(_axisUpOS, _axisRightOS.normalized);
        planeVisual.transform.localPosition = _centerPositionOS;
        planeVisual.transform.localRotation = Quaternion.LookRotation(Vector3.Cross(_axisRightOS, _axisUpOS), _axisUpOS);
        planeVisual.OnUpdate();

        planeVisual.SetCursor(positionUV * size);
    }

    bool RayIntersectsPlane(Vector3 planePoint, Vector3 planeNormal, Vector3 rayOrigin, Vector3 rayDirection, out Vector3 intersectionPoint)
    {
        intersectionPoint = Vector3.zero;
        
        // 평면 법선 벡터와 직선 방향 벡터의 내적 계산
        float denominator = Vector3.Dot(planeNormal, rayDirection);
        
        // 내적이 0이면 직선이 평면과 평행하거나 평면에 속함
        if (Mathf.Abs(denominator) < 1e-6)
        {
            return false; // 교점 없음
        }

        // 매개변수 t 계산
        float t = Vector3.Dot(planeNormal, planePoint - rayOrigin) / denominator;
        
        // 교점 계산
        intersectionPoint = rayOrigin + t * rayDirection;
        
        return true; // 교점 있음
    }
}
