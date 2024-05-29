using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManager;



[ExecuteInEditMode]
public class PointInteractable : MonoBehaviour
{
    public FingerInteractable[] pointInteractable = new FingerInteractable[5];

    
    void Awake() {
        pointInteractable[0].name = "Thumb";
        pointInteractable[1].name = "Index";
        pointInteractable[2].name = "Middle";
        pointInteractable[3].name = "Ring";
        pointInteractable[4].name = "Pinky";
    }

    void SetEndTransformEndBezier(BezierLine bezierLine) {
        bezierLine.endTransformWS = this.transform;
    }
    
}
