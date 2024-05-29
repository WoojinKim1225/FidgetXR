using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManager;



public class ObjectInteractable : MonoBehaviour
{
    public Quaternion beforeRotation;
    public Quaternion rotationSpeed;

    public Vector3 rotationAxis;
    public float rotationAngle;

    public TangibleInteractable tangibleInteractable = new TangibleInteractable();

    void Update()
    {
        rotationSpeed = Quaternion.Inverse(beforeRotation) * transform.rotation;
        rotationSpeed.ToAngleAxis(out rotationAngle, out rotationAxis);
        beforeRotation = transform.rotation;
    }
}
