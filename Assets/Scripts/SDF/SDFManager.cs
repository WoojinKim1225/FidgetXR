using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class SDFManager : MonoBehaviour
{
    public Material SDFMaterial;

    public enum EInteractionMode {
        None = 0,
        Button = 1,
        Slider = 2,
        Knob = 3,
        ScreenTouch = 4,
        ScreenPinch = 5,
    }
    public EInteractionMode interactionMode;

    private Dictionary<EInteractionMode, Matrix4x4> matrixDict = new Dictionary<EInteractionMode,Matrix4x4>{
        { EInteractionMode.None, new Matrix4x4(new Vector4(1,0,0,0), Vector4.zero, Vector4.zero, Vector4.zero)},
        { EInteractionMode.Button, new Matrix4x4(new Vector4(0,1,0,0), Vector4.zero, Vector4.zero, Vector4.zero)},
        { EInteractionMode.Slider, new Matrix4x4(new Vector4(0,0,1,0), Vector4.zero, Vector4.zero, Vector4.zero)},
        { EInteractionMode.Knob, new Matrix4x4(new Vector4(0,0,0,1), Vector4.zero, Vector4.zero, Vector4.zero)},
        { EInteractionMode.ScreenTouch, new Matrix4x4(Vector4.zero, new Vector4(1,0,0,0), Vector4.zero, Vector4.zero)},
        { EInteractionMode.ScreenPinch, new Matrix4x4(Vector4.zero, new Vector4(0,1,0,0), Vector4.zero, Vector4.zero)},

    };

    private Matrix4x4 mode => matrixDict[interactionMode];
    private Matrix4x4 beforeMode, smoothMode;

    public Vector3 position;
    public Vector3 direction;
    public float radiusStart;

    public Transform thumbTrs, indexTrs, middleTrs, ringTrs, pinkyTrs;
    
    // Start is called before the first frame update
    void Start()
    {
        beforeMode = mode;
        smoothMode = mode;
    }

    // Update is called once per frame
    void Update()
    {
        position = (thumbTrs.position + indexTrs.position) / 2f;

        radiusStart = Mathf.Min(Vector3.Distance(thumbTrs.position, indexTrs.position) / 2f - 0.01f, 0.02f);
        radiusStart = Mathf.Max(radiusStart, 0.002f);
        SDFMaterial.SetVector("_Position", position);
        SDFMaterial.SetVector("_Direction", direction);
        SDFMaterial.SetFloat("_Radius", radiusStart);
        SDFMaterial.SetMatrix("_Mode", mode);
    }

    private Matrix4x4 smooth(Matrix4x4 a, Matrix4x4 b, float t) {
        return new Matrix4x4(
            Vector4.Lerp(a.GetColumn(0), b.GetColumn(0), t),
            Vector4.Lerp(a.GetColumn(1), b.GetColumn(1), t),
            Vector4.Lerp(a.GetColumn(2), b.GetColumn(2), t),
            Vector4.Lerp(a.GetColumn(3), b.GetColumn(3), t)
        );
    }
}
