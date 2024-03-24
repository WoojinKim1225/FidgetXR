using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointData : MonoBehaviour
{
    public EInteractableFinger InteractableFinger;
    [SerializeField] private UnityEventButton ThumbIndex;
    [SerializeField] private UnityEventButton ThumbMiddle;
    [SerializeField] private UnityEventButton ThumbRing;
    [SerializeField] private UnityEventButton ThumbPinky;

    public List<UnityEventButton> unityEventButtons;
    // Start is called before the first frame update
    void Start()
    {
        unityEventButtons = new List<UnityEventButton>(4);
        unityEventButtons[0] = ThumbIndex;
        unityEventButtons[1] = ThumbMiddle;
        unityEventButtons[2] = ThumbRing;
        unityEventButtons[3] = ThumbPinky;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
