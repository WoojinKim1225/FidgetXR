using UnityEngine;
using InteractableHelper;

public class LineInteractable : MonoBehaviour
{
    public LineRenderer lineVisualizer;

    public enum ELineType {
        Segment, Bezier
    }
    public ELineType lineType = ELineType.Segment;

    public FingerInteractable[] lineInteractable = new FingerInteractable[5];

    
    void Awake() {
        lineInteractable[0].name = "Thumb";
        lineInteractable[1].name = "Index";
        lineInteractable[2].name = "Middle";
        lineInteractable[3].name = "Ring";
        lineInteractable[4].name = "Pinky";

        if (lineType == ELineType.Segment) {
            _value = null;
        } else {
            _value = 0;
        }
    }


    private float? _value;
    public float NumValue => _value.GetValueOrDefault();
    public bool HasValue => _value.HasValue;

    public bool isQuantized = false;
    public float quantizeStep;

    [SerializeField] private Vector3 _startPositionOS, _endPositionOS;

    private bool _isSegmentGrabPositionKnown = false;
    private bool _isSegmentGrabSideStart = false;
    [SerializeField] private bool _updatePosition;

    void Update()
    {
        if (!_updatePosition) return;
        lineVisualizer.SetPosition(0, _startPositionOS);
        lineVisualizer.SetPosition(1, _endPositionOS);
    }


    public float GetRelativePosition01(Vector3 pos) {
        return Mathf.Clamp01(Vector3.Dot(_endPositionOS - _startPositionOS, transform.InverseTransformPoint(pos) - _endPositionOS) / Vector3.SqrMagnitude(_endPositionOS - _startPositionOS));
    }

    public void SetStartPosition(Transform trs) {
        _startPositionOS = transform.InverseTransformPoint(trs.position);
    }

    public void SetStartPosition(Vector3 pos) {
        _startPositionOS = transform.InverseTransformPoint(pos);
    }

    public void SetEndPosition(Transform trs) {
        _endPositionOS = transform.InverseTransformPoint(trs.position);
    }

    public void SetEndPosition(Vector3 pos) {
        _endPositionOS = transform.InverseTransformPoint(pos);
    }

    
    public void GrabSegmentPosition(Vector3 pos) {
        if (!_isSegmentGrabPositionKnown) {
            float f = GetRelativePosition01(pos);
            _isSegmentGrabSideStart = f < 0.5f;
            _isSegmentGrabPositionKnown = true;
        }

        if (_isSegmentGrabSideStart) {
            SetStartPosition(pos);
        } else {
            SetEndPosition(pos);
        }
    }

    public void UnGrabSegmentPosition() {
        _isSegmentGrabPositionKnown = false;
        _isSegmentGrabSideStart = false;
    }

    public void GrabValue(Vector3 pos) {
        if (!_value.HasValue) {
            return;
        }
        float f = GetRelativePosition01(pos);
        
        _value = f;
    }

    public void QuantizeValue(float v) {
        if (!isQuantized) return;
        _value = Mathf.Round(v / quantizeStep) * quantizeStep;
    }

    
}
