using System;
using UnityEngine;
using UnityEngine.Events;
 
[System.Serializable]
public struct UnityEventButton {
    public UnityEvent OnPress, OnRelease, OnClick;
}

[Flags]
public enum EInteractableFinger {
    None = 0,
    Thumb = 1,
    Index = 2,
    Middle = 4,
    Ring = 8,
    Pinky = 16
}

[Flags]
public enum EInteractType {
    None = 0,
    Touch = 1,
    FingerCurl = 2,
    FingerTouch = 4,
    Swipe = 8,
    Twist = 16,
    Translate = 32,
    Rotate = 64,
}

public class InteractionUtils : MonoBehaviour
{
    
}