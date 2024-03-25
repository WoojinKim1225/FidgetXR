using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Segment {
    public int fingerNum;
    public Vector3 startPositionOS, endPositionOS;
    public bool isQuantized;
    public int quantizeValue;
    public Vector3 state;
    public float state01;
}

public class SegmentData : MonoBehaviour
{
    public Segment segment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable() {
        
    }
}
