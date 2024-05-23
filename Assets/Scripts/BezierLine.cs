using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierLine : MonoBehaviour
{
    [SerializeField] private Vector3 _startPoint, _endPoint, _midPoint;
    private Vector3 _midPointB;

    public Transform startTransformWS, endTransformWS;

    public int segCount;

    private LineRenderer lineRenderer;
    public bool Visibility {set => lineRenderer.enabled = value; }

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segCount;
    }
    
    private void Update() {
        if (endTransformWS != null) {
            _startPoint = startTransformWS.position;
            _endPoint = endTransformWS.position;
            _midPoint = startTransformWS.position + startTransformWS.forward * Vector3.Magnitude(endTransformWS.position - startTransformWS.position) * 0.5f;
        }

        if (!lineRenderer.isVisible) return;
        if (segCount != lineRenderer.positionCount) {
            lineRenderer.positionCount = segCount;
        }
        if (_startPoint == lineRenderer.GetPosition(0) && _endPoint == lineRenderer.GetPosition(segCount - 1) && _midPoint == _midPointB) return;

        lineRenderer.SetPositions(Enumerable.Range(0, segCount)
                                   .Select(i => GetBezierPoint(_startPoint, _midPoint, _endPoint, i / (segCount - 1.0f)))
                                   .ToArray());
        
        _midPointB = _midPoint;
    }

    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;

        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}
