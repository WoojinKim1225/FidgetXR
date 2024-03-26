using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Point {
    public bool isToggle;

    public UnityEventButton unityEventButton;
}

public class PointData : MonoBehaviour
{
    [SerializeField] private List<Point> points;
    
    // The positions of each points in Object Space.
    public List<Vector3> pointPositionOS;

    public EConnectFinger connectFinger;
    public uint connectedFinger;

    public uint states;
    private uint statesBefore;

    private uint onStates, offStates;

    [SerializeField] private bool isFingerDependent;

    public void setStates(int bit, int value){
        if (value == 0) {
            states &= ~(1u << bit);
        } else {
            states |= 1u << bit;
        }
    }
    
    void Update()
    {
        if (statesBefore != states) {
            onStates = states & ~statesBefore;
            offStates = ~states & statesBefore;
            Debug.Log(onStates.ToString() + ", " +  offStates.ToString());
            

            if (!isFingerDependent) {
                for (int i = 0; i < points.Count; i++) {
                    if (((onStates >> i) & 1) == 1) {
                        points[i].unityEventButton.OnPress.Invoke();
                        buttonClickIndependent(i);
                    }
                    if (((offStates >> i) & 1) == 1) {
                        points[i].unityEventButton.OnRelease.Invoke();
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
            points[value].unityEventButton.OnClick.Invoke();
        }
    }

}
