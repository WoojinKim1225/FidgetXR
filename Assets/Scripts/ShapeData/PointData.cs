using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Point {
    public Vector3 positionOS;
    public bool isToggle;
    public bool state;
}

public class PointData : ShapeInteractable
{
    public List<Point> points;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
