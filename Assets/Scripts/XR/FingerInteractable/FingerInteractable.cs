using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FingerActions {
    [System.Flags] public enum EFingerNumber{None = 0, Thumb = 1, Index = 2, Middle = 4, Ring = 8, Pinky = 16}

    /// <summary>
    /// defines which finger can interact with this object;
    /// </summary>
    public EFingerNumber fingerMask;

    /// <summary>
    /// this event occurs when the Angle between proximal and distal is greater than threshold;
    /// </summary>
    public UnityEventStateful curl;

    /// <summary>
    /// this event occurs when the Distance Function of the destination and finger tip is lesser than threshold;
    /// </summary>
    public UnityEventStateful touch;

    /// <summary>
    /// this event occur when the Distance of the thumb tip and corresponding finger tip is lesser than threshold. thumb finger won't execute this event;
    /// </summary>
    public UnityEventStateful pinch;

    /// <summary>
    /// this event occur when the Distance of the thumb tip and corresponding finger proximal is lesser than threshold. thumb finger won't execute this event;
    /// </summary>
    public UnityEventStateful tap;
}

public abstract class FingerInteractable : MonoBehaviour
{
    public FingerActions fingerActions;

    public abstract float DistanceFunction(Transform from);
}