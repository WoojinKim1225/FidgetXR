using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Point {
    public Vector3 positionOS;
    public bool isToggle;
    public bool state;

    public UnityEventButton unityEventButton;
}

public class PointData : MonoBehaviour
{
    public List<Point> points;
    public EInteractableFinger interactableFinger;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
