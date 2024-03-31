using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamera : MonoBehaviour
{
    public InputActionProperty CameraMove;
    public InputActionProperty CameraRotate;
    private Vector2 rotation;

    void Awake()
    {
        CameraMove.action.Enable();
        CameraRotate.action.Enable();
    }
    void OnDisable()
    {
        CameraMove.action.Disable();
        CameraRotate.action.Disable();
    }

    void Update()
    {
        transform.position += transform.TransformDirection(CameraMove.action.ReadValue<Vector3>()) * Time.deltaTime;
        rotation += new Vector2(-CameraRotate.action.ReadValue<Vector2>().y, CameraRotate.action.ReadValue<Vector2>().x) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);
    }
}

