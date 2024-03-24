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

public class InteractionUtils : MonoBehaviour
{
    
}