using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RaymarchCamera : MonoBehaviour
{
    private Camera _camera;
    private float nearClipPlane, fieldOfView, aspect;

    void Awake()
    {
        _camera = GetComponentInParent<Camera>();
    }
    private void LateUpdate() {
        if(nearClipPlane == _camera.nearClipPlane && fieldOfView == _camera.fieldOfView && aspect == _camera.aspect) return;
        
        nearClipPlane = _camera.nearClipPlane;
        fieldOfView = _camera.fieldOfView;
        aspect = _camera.aspect;
        transform.localPosition = Vector3.forward * (_camera.nearClipPlane + 1f/128f);
        float viewPortScale = 2 * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * (_camera.nearClipPlane + 1f/128f);
        transform.localScale = new Vector3(viewPortScale * _camera.aspect, viewPortScale, 1);
        
    }
}
