using UnityEngine;
using UnityEngine.Events;

namespace InteractableHelper
{
    [System.Serializable]
    public struct StatefulButtonEvent {
        public UnityEvent OnEnter, OnStay, OnExit, OnClick;
    }

    [System.Serializable]
    public struct StatefulAxisEvent {
        public float value;
        public StatefulButtonEvent button;
        public UnityEvent OnValueChange;
    }

    [System.Serializable]
    public struct StatefulVector2Event {
        public Vector2 value;
        public StatefulButtonEvent button;
        public UnityEvent OnValueChange;
    }

    [System.Serializable]
    public struct StatefulPivotEvent {
        public Vector3 pivot;
        public StatefulButtonEvent button;
    }

    [System.Serializable]
    public struct FingerInteractable {
        [HideInInspector]
        public string name;

        // Curling One Finger
        public StatefulButtonEvent Curl;

        // Pinching Corresponding Finger with Thumb;
        public StatefulButtonEvent Pinch;

        // Tip Distance From Object;
        public StatefulButtonEvent Touch;
        
        // Tapping Finger Base with Thumb;
        public StatefulButtonEvent Tap;
    }
}