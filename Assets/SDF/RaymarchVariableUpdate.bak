using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchVariableUpdate : MonoBehaviour
{
    public FullScreenPassRendererFeature fullScreenPassRendererFeature;
    public bool isRight;
    public Vector3? pointerEnd = null;

    public Transform palmPointerHolder;
    private Vector3 deltaTarget;

    public OVRSkeleton skeleton;

    void Update()
    {
        if (!skeleton.IsDataValid) return;

        Transform thisTransform = transform;

        Vector3 a = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Thumb3].Transform.position;
        Vector3 b = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index3].Transform.position;
        Vector3 c = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle3].Transform.position;

        Vector3 midIndexThumb = Vector3.Lerp((a + b) / 2f, c, 0.25f);
        

        float A = Vector3.Distance(skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position, skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position);
        float B = Vector3.Distance(skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position, skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform.position);
        
        Vector3 pos = Vector3.Lerp(thisTransform.position, midIndexThumb, 1f);
        
        if (!pointerEnd.HasValue) {
            deltaTarget = Camera.main.transform.forward;
        } else {
            deltaTarget = (pointerEnd.Value - pos).normalized;
        }
        
        float r = Math.Min(A,B) * 0.2f;
        Vector3 dir = deltaTarget * 0.03f;
        
        if (isRight)
        {
            float palmView = Mathf.Clamp01(Vector3.Dot(transform.up, Camera.main.transform.forward));
            fullScreenPassRendererFeature.passMaterial.SetVector("_RightHandPointerPos", pos);
            fullScreenPassRendererFeature.passMaterial.SetFloat("_RightHandPointerR", r);
            fullScreenPassRendererFeature.passMaterial.SetVector("_RightHandPointerDir", dir);
            fullScreenPassRendererFeature.passMaterial.SetFloat("_RightPalmView", palmView);
        }
        else
        {
            float palmView = Mathf.Clamp01(-Vector3.Dot(transform.up, Camera.main.transform.forward));
            fullScreenPassRendererFeature.passMaterial.SetVector("_LeftHandPointerPos", pos);
            fullScreenPassRendererFeature.passMaterial.SetFloat("_LeftHandPointerR", r);
            fullScreenPassRendererFeature.passMaterial.SetVector("_LeftHandPointerDir", dir);
            fullScreenPassRendererFeature.passMaterial.SetFloat("_RightPalmView", palmView);
        }
    }

    Vector3 FindCenter(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var t = p2 - p1;
        var u = p3 - p1;
        var v = p3 - p2;

        return p1 + t * 0.2f + u * 0.2f;
    }

    Vector3 FindCircCenter(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var t = p2 - p1;
        var u = p3 - p1;
        var v = p3 - p2;

        // triangle normal
        var w = Vector3.Cross(t, u);
        var wsl = Vector3.Dot(w, w);
        // TODO: if (wsl<10e-14) return false; // area of the triangle is too small (you may additionally check the points for colinearity if you are paranoid)

        // helpers
        var iwsl2 = 1f / (2f * wsl);
        var tt = Vector3.Dot(t, t);
        var uu = Vector3.Dot(u, u);

        // result circle
        Vector3 circCenter = p1 + (u * tt * (Vector3.Dot(u, v)) - t * uu * (Vector3.Dot(t, v))) * iwsl2;
        var     circRadius = Mathf.Sqrt(tt * uu * (Vector3.Dot(v, v)) * iwsl2 * 0.5f);
        Vector3 circAxis   = w / Mathf.Sqrt(wsl);
        
        return circCenter;
    }

}


