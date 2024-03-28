using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct Quad {
    public UnityEventQuad unityEventButton;
}

public class QuadData : MonoBehaviour
{
    [SerializeField] private List<Quad> quad;

    public Vector3 startPointOS, rightOS, upOS;
    private Vector3 startPointWS, rightWS, upWS;

    public EConnectFinger connectFinger;
    public uint connectedFinger;

    public Vector2 position;
    private Vector2 positionBefore;

    public uint states;
    private uint statesBefore;

    private uint onStates, offStates;

    [SerializeField] private bool isFingerDependent;

    public Vector3 fingerPositionWS;

    void Start()
    {
        positionBefore = position;
    }

    private void Update() {
        if (states != statesBefore) {
            onStates = states & ~statesBefore;
            offStates = ~states & statesBefore;
            
            if (!isFingerDependent) {
                for (int i = 0; i < quad.Count; i++) {
                    if (((onStates >> i) & 1) == 1) {
                        quad[i].unityEventButton.OnPress.Invoke();
                        buttonClickIndependent(i);
                    }
                    if (((offStates >> i) & 1) == 1) {
                        quad[i].unityEventButton.OnRelease.Invoke();
                    }
                }
            } else {
                
            }

            statesBefore = states;
        }

    }

    public IEnumerator buttonClickIndependent(int value) {
        yield return new WaitForSeconds(0.5f);
        if (((offStates >> value) & 1) == 1) {
            quad[value].unityEventButton.OnClick.Invoke();
        }
    }
}
