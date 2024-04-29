using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct StatefulUnityEvent {
    public UnityEvent OnEnter, OnStay, OnExit, OnClick;
}

[Serializable]
public struct FingerInteractable {
    [HideInInspector]
    public string name;

    // Curling One Finger
    public StatefulUnityEvent Curl;

    // Pinching Corresponding Finger with Thumb;
    public StatefulUnityEvent Pinch;

    // Tip Distance From Object;
    public StatefulUnityEvent Touch;
    
    // Tapping Finger Base with Thumb;
    public StatefulUnityEvent Tap;
}

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

    void SetEndTransform(BezierLine bezierLine) {
        bezierLine.endTransformWS = this.transform;
    }
    
}
