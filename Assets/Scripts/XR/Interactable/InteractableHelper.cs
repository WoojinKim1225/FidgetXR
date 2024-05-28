using UnityEngine;
using UnityEngine.Events;

namespace EventManager
{
    [System.Serializable]
    public struct StatefulButtonUnityEvent {
        public UnityEvent OnEnter, OnStay, OnExit, OnClick;
    }

    [System.Serializable]
    public struct StatefulAxisUnityEvent {
        public UnityEvent OnValueChange;
    }

    [System.Serializable]
    public struct StatefulDirectionUnityEvent {
        public Vector2 axis;
        public StatefulButtonUnityEvent button;
    }

    [System.Serializable]
    public struct FingerInteractable {
        [HideInInspector]
        public string name;

        // Curling One Finger
        public StatefulButtonUnityEvent Curl;

        // Pinching Corresponding Finger with Thumb;
        public StatefulButtonUnityEvent Pinch;

        // Tip Distance From Object;
        public StatefulButtonUnityEvent Touch;
        
        // Tapping Finger Base with Thumb;
        public StatefulButtonUnityEvent Tap;
    }

    [System.Serializable]
    public struct TangibleInteractable {
        [HideInInspector]
        public string name;

        public StatefulAxisUnityEvent pitch, yaw, roll;
        public StatefulAxisUnityEvent sway, heave, surge;
    }
}