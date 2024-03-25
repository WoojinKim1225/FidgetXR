using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ShapeInteractable : MonoBehaviour {
    public List<UnityEventButton> unityEventButtons;

    public EInteractableFinger interactableFinger;
    public EInteractableFinger interactingFinger = EInteractableFinger.None;
}