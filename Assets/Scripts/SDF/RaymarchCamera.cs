using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RaymarchCamera : MonoBehaviour
{
    [SerializeField] private Shader _shader;

    [SerializeField] private Material _raymarchMaterial;
    public Material RaymarchMaterial {
        get {
            if (!_raymarchMaterial && _shader) {
                _raymarchMaterial = new Material(_shader);
                _raymarchMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return _raymarchMaterial;
        }
    }

    private Camera _camera;
    public Camera Camera {
        get{
            if (!_camera) {
                _camera = GetComponent<Camera>();
            }
            return _camera;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (!_raymarchMaterial) {
            Graphics.Blit(src, dest);
            return;
        }
        _raymarchMaterial.SetMatrix("_CameraFrustum", GetCameraFrustum(_camera));
        _raymarchMaterial.SetMatrix("_CStoWS", _camera.cameraToWorldMatrix);
        _raymarchMaterial.SetVector("_CameraWS", _camera.transform.position);

        RenderTexture.active = dest;
        GL.PushMatrix();
        GL.LoadOrtho();
        _raymarchMaterial.SetPass(0);
        GL.Begin(GL.QUADS);

        //bl
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f);

        //br
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f);

        //tr
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f);

        //tl
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
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
