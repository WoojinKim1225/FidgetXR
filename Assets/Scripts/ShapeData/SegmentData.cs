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
    public Segment segment;

    public Vector3 startPositionOS, endPositionOS;
    private Vector3 startPositionWS => transform.TransformPoint(startPositionOS);
    private Vector3 endPositionWS => transform.TransformPoint(endPositionOS);

    public EConnectFinger connectFinger;
    public uint connectedFinger;

    [Range(0,1)]
    public float state01;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetStateFromFingerPosition(Vector3 fingerPositionWS) {
        Vector3 AB = endPositionWS - startPositionWS;
        Vector3 AC = fingerPositionWS - startPositionWS;
        //float AD_distance = 
    }
}
