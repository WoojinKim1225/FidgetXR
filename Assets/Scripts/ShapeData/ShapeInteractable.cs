using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ShapeInteractable : MonoBehaviour {
    public List<UnityEventButton> unityEventButtons;

    public Dictionary<int, UnityEventButton> unityEventButtonsDictionary;

    public EInteractableFinger interactableFinger;
    public EInteractableFinger interactingFinger = EInteractableFinger.None;

    private void Awake() {
        int j = 0;
        unityEventButtonsDictionary = null;
        for (int i = 0; i < 5; i++) {
            if ((((int)interactableFinger >> i) & 1) == 1) {
                unityEventButtonsDictionary.Add(i, unityEventButtons[j]);
                j++;
            }
        }
    }
}