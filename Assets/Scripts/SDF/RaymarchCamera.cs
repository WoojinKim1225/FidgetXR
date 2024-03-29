using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RaymarchCamera : MonoBehaviour
{
    [SerializeField]private Camera _camera;

    private void LateUpdate() {
        transform.localPosition = Vector3.forward * (_camera.nearClipPlane + 1f/128f);
        float viewPortScale = 2 * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * (_camera.nearClipPlane + 1f/128f);
        transform.localScale = new Vector3(viewPortScale * _camera.aspect, viewPortScale, 1);
    }

    private Matrix4x4 GetCameraFrustum(Camera c)
    {
        Matrix4x4 f = Matrix4x4.identity;
        float fov = Mathf.Tan(c.fieldOfView * 0.5f * Mathf.Deg2Rad);
        Vector3 u = Vector3.up * fov;
        Vector3 r = Vector3.right * fov * c.aspect;

        Vector3 tl = Vector3.back - r + u;
        Vector3 tr = Vector3.back + r + u;
        Vector3 bl = Vector3.back - r - u;
        Vector3 br = Vector3.back + r - u;

        f.SetRow(0, tl);
        f.SetRow(1, tr);
        f.SetRow(2, br);
        f.SetRow(3, bl);

        return f;
    }
}
