using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractableHelper;
using UnityEngine.Events;

public class VolumeInteractable : MonoBehaviour
{
    private Vector3 _cameraPositionWS;
    private Vector3 _cameraDirectionWS;

    public List<StatefulPivotEvent> pivots = new List<StatefulPivotEvent>();
    private float maxValue;
    private int maxElement;
    private int maxBeforeElement;
    // Start is called before the first frame update
    void Start()
    {
        maxElement = -1;
    }

    // Update is called once per frame
    void Update()
    {
        _cameraPositionWS = Camera.main.transform.position;
        _cameraDirectionWS = -Camera.main.transform.forward;
        maxBeforeElement = maxElement;
        maxValue = float.MinValue;

        for (int i = 0; i < pivots.Count; i++) {
            float value = Vector3.Dot(transform.TransformDirection(pivots[i].pivot), (_cameraPositionWS - transform.position).normalized);
            if (maxValue < value) {
                maxValue = value;
                maxElement = i;
            }
        }

        for (int i = 0; i < pivots.Count; i++) {
            if (i == maxBeforeElement && i != maxElement) {
                pivots[i].button.OnExit.Invoke();
            } else if (i != maxBeforeElement && i == maxElement) {
                pivots[i].button.OnEnter.Invoke();
            } else if (i== maxBeforeElement && i == maxElement) {
                pivots[i].button.OnStay.Invoke();
            }
        }
    }

    private Color Dir2Color(Vector3 d) {
        Vector3 n = (d.normalized + Vector3.one) * 0.5f;
        Color c = new Color(n.x, n.y, n.z, 1f); 
        return c;
    }
}
