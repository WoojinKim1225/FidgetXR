using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Arc : FingerInteractable
{
    [SerializeField] Vector3 _centerPositionOS = Vector3.zero;
    [SerializeField] Vector3 _axisOS = Vector3.up;
    private Vector3 _axisOSBefore;
    [SerializeField] Vector3 _axis2OS = Vector3.forward;
    private Vector3 _axis2OSBefore;
    [SerializeField] float _startAngle = -90f;
    private float _startAngleBefore;
    [SerializeField] float _endAngle = 90f;
    private float _endAngleBefore;
    [SerializeField] float _radius = 0.01f;
    private float _radiusBefore;

    public float Radius => _radius;
    public float StartAngle => _startAngle;
    public float EndAngle => _endAngle;

    public LineRenderer arcVisual;
    public ArcMesh arcValue;
    [SerializeField] int _arcSegmentCount = 3;
    public int ArcSegmentCount => _arcSegmentCount;
    private int _arcSegmentCountBefore;
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
        Vector3 directionWS = Vector3.ProjectOnPlane(from.position - transform.TransformPoint(_centerPositionOS), transform.TransformDirection(_axisOS)).normalized;
        positionWSUnclamped = transform.TransformPoint(_centerPositionOS) + directionWS * _radius;
        positionAngleUnclamped = Vector3.SignedAngle(transform.TransformDirection(_axis2OS), directionWS, transform.TransformDirection(_axisOS));
        position01Unclamped = Mathf.InverseLerp(_startAngle, _endAngle, positionAngleUnclamped);
        positionOSUnclamped = transform.InverseTransformPoint(positionWSUnclamped);

        position01 = Mathf.Clamp01(position01Unclamped);
        positionAngle = Mathf.Lerp(_startAngle, _endAngle, position01);
        positionOS = Quaternion.AngleAxis(positionAngle, _axisOS) * _axis2OS * _radius;
        positionWS = transform.TransformPoint(positionOS);

        return Vector3.Distance(from.position, positionWS);
    }

    void Awake()
    {
        arcVisual = GetComponentInChildren<LineRenderer>();
        arcValue = GetComponentInChildren<ArcMesh>();
    }

    public void Update()
    {
        valueAngle.OnUpdate();
        _axisOS = Vector3.Normalize(_axisOS);
        _axis2OS = Vector3.Normalize(Vector3.ProjectOnPlane(_axis2OS, _axisOS));

        arcVisual.positionCount = _arcSegmentCount + 1;
        arcVisual.transform.rotation = transform.rotation;
        arcVisual.transform.localPosition = _centerPositionOS;

        if (_startAngle != _startAngleBefore || _endAngle != _endAngleBefore || _arcSegmentCount != _arcSegmentCountBefore || _axis2OS != _axis2OSBefore || _axisOS != _axisOSBefore || _radius != _radiusBefore) {
            for (int i = 0; i < arcVisual.positionCount; i++) {
                float angle = Mathf.Lerp(_startAngle, _endAngle, i * 1.0f / _arcSegmentCount);
                Vector3 posOS = Quaternion.AngleAxis(-angle, _axisOS.normalized) * _axis2OS.normalized * _radius;
                arcVisual.SetPosition(i, posOS);
            }
            arcValue.OnUpdate();
            _startAngleBefore = _startAngle;
            _endAngleBefore = _endAngle;
            _arcSegmentCountBefore = _arcSegmentCount;
            _axisOSBefore = _axisOS;
            _axis2OSBefore = _axis2OS;
            _radiusBefore = _radius;
        }

        valueVisual.sharedMaterial.SetFloat("_From", _startAngle);
        valueVisual.sharedMaterial.SetFloat("_To", valueAngle.value);
    }
}
