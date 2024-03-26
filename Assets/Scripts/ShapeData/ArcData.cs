using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Arc {
    public Vector3 centerPositionOS;
    public Vector3 arcUpOS;
    public Vector3 arcForwardOS;
    public Vector2 arcAngle;
    public bool isQuantized;
    public int quantizeValue;
    public Vector3 state;
    public float state01;
}

public class ArcData : ShapeInteractive
{
    public Arc arc;
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
