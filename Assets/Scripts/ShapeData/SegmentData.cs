using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Segment {
    public bool isQuantized;
    public int quantizeValue;
    
    public UnityEventSlider unityEventSlider;
}

public class SegmentData : MonoBehaviour
{
    [SerializeField] private Segment segment;

    public Vector3 startPositionOS, endPositionOS;
    private Vector3 startPositionWS, endPositionWS;

    public EConnectFinger connectFinger;
    public uint connectedFinger;

    [Range(0,1)]
    public float state01;
    private float state01before;

    public bool isSelect;
    private bool isSelected;

    public Vector3 fingerPositionWS;
    
    // Start is called before the first frame update
    void Start()
    {
        state01before = state01;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect) GetStateFromFingerPosition();
        if (isSelect && !isSelected) {
            segment.unityEventSlider.OnSelect.Invoke();
            isSelected = true;
        }
        if (!isSelect && isSelected) {
            segment.unityEventSlider.OnDeselect.Invoke();
            isSelected = false;
        }

        if (state01 != state01before) {
            if (state01 == 0) {
                segment.unityEventSlider.OnZero.Invoke();
            }
            if (state01 == 1) {
                segment.unityEventSlider.OnOne.Invoke();
            }
            segment.unityEventSlider.OnValueChange.Invoke();
            state01before = state01;
        }
    }

    void GetStateFromFingerPosition() {
        startPositionWS = transform.TransformPoint(startPositionOS);
        endPositionWS = transform.TransformPoint(endPositionOS);
        Vector3 AB = endPositionWS - startPositionWS;
        Vector3 AC = fingerPositionWS - startPositionWS;
        float t = Vector3.Dot(AB, AC) / Vector3.Dot(AB, AB);
        if (segment.isQuantized) {
            t = Mathf.Round(t * segment.quantizeValue) / segment.quantizeValue;
        }
        state01 = Mathf.Clamp01(t);
    }
}
