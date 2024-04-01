using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchPointer : MonoBehaviour
{
    [SerializeField] private FullScreenPassRendererFeature _raymarchRendererFeature;

    public Vector3? pointerEnd_L = null;
    public Vector3? pointerEnd_R = null;

    [SerializeField] private OVRSkeleton skeleton_L, skeleton_R;
    [SerializeField] private FingerCursor cursor_L, cursor_R;
    private Transform pointerHolder_L, pointerHolder_R;
    [SerializeField] private Vector3 pointerOffset_L, pointerOffset_R;
    [SerializeField] private Vector3 palmOffset_L, palmOffset_R;
    private Vector3 deltaTarget_L, deltaTarget_R;
    private Vector3[] l, r;

    void Awake()
    {
        l = new Vector3[5];
        r = new Vector3[5];
    }


    // Update is called once per frame
    void Update()
    {
        if (!skeleton_L.IsDataValid || !skeleton_R.IsDataValid) return;

        UpdateFingerPos();

        pointerHolder_L = cursor_L.transform;
        pointerHolder_R = cursor_R.transform;

        Vector3 midIndexThumb_l = Vector3.Lerp(l[0], l[1], 0.5f) + Vector3.ProjectOnPlane(l[2] - l[1], l[1] - l[0]) * 0.5f;
        Vector3 midIndexThumb_r = Vector3.Lerp(r[0], r[1], 0.5f) + Vector3.ProjectOnPlane(r[2] - r[1], r[1] - r[0]) * 0.5f;
        Vector3 sum_l = l[0] + l[1] + l[2] + l[3] + l[4];
        Vector3 sum_r = r[0] + r[1] + r[2] + r[3] + r[4];

        Vector3 pos0_L = Vector3.Lerp(pointerHolder_L.TransformPoint(pointerOffset_L), midIndexThumb_l, 0.5f);
        Vector3 pos0_R = Vector3.Lerp(pointerHolder_R.TransformPoint(pointerOffset_R), midIndexThumb_r, 0.5f);

        Vector3 pos1_L = Vector3.Lerp(pointerHolder_L.TransformPoint(palmOffset_L), sum_l * 0.2f, 0.5f);
        Vector3 pos1_R = Vector3.Lerp(pointerHolder_R.TransformPoint(palmOffset_R), sum_r * 0.2f, 0.5f);

        float palmView_L = Mathf.Clamp01(Vector3.Dot(-pointerHolder_L.up, Camera.main.transform.forward));
        float palmView_R = Mathf.Clamp01(Vector3.Dot(pointerHolder_R.up, Camera.main.transform.forward));

        Vector3 pos_L = Vector3.Lerp(pos0_L, pos1_L, palmView_L);
        Vector3 pos_R = Vector3.Lerp(pos0_R, pos1_R, palmView_R);

        deltaTarget_L = pointerEnd_L.HasValue ? (pointerEnd_L.Value - pos0_L).normalized : Camera.main.transform.forward;
        deltaTarget_R = pointerEnd_R.HasValue ? (pointerEnd_R.Value - pos0_R).normalized : Camera.main.transform.forward;

        Vector3 dir_L = deltaTarget_L * 0.03f;
        Vector3 dir_R = deltaTarget_R * 0.03f;

        //float r_L = Math.Min(,B) * 0.2f;

        

        _raymarchRendererFeature.passMaterial.SetVector("_LeftHandPointerPos", pos_L);
        _raymarchRendererFeature.passMaterial.SetVector("_RightHandPointerPos", pos_R);
        _raymarchRendererFeature.passMaterial.SetFloat("_LeftHandPointerR", 0.03f);
        _raymarchRendererFeature.passMaterial.SetFloat("_RightHandPointerR", 0.03f);
        _raymarchRendererFeature.passMaterial.SetVector("_LeftHandPointerDir", dir_L);
        _raymarchRendererFeature.passMaterial.SetVector("_RightHandPointerDir", dir_R);
        _raymarchRendererFeature.passMaterial.SetFloat("_LeftPalmView", palmView_L);
        _raymarchRendererFeature.passMaterial.SetFloat("_RightPalmView", palmView_R);

    }

    void UpdateFingerPos() 
    {
        l[0] = skeleton_L.Bones[(int)OVRSkeleton.BoneId.Hand_Thumb3].Transform.position;
        l[1] = skeleton_L.Bones[(int)OVRSkeleton.BoneId.Hand_Index3].Transform.position;
        l[2] = skeleton_L.Bones[(int)OVRSkeleton.BoneId.Hand_Middle3].Transform.position;
        l[3] = skeleton_L.Bones[(int)OVRSkeleton.BoneId.Hand_Ring3].Transform.position;
        l[4] = skeleton_L.Bones[(int)OVRSkeleton.BoneId.Hand_Pinky3].Transform.position;
        r[0] = skeleton_R.Bones[(int)OVRSkeleton.BoneId.Hand_Thumb3].Transform.position;
        r[1] = skeleton_R.Bones[(int)OVRSkeleton.BoneId.Hand_Index3].Transform.position;
        r[2] = skeleton_R.Bones[(int)OVRSkeleton.BoneId.Hand_Middle3].Transform.position;
        r[3] = skeleton_R.Bones[(int)OVRSkeleton.BoneId.Hand_Ring3].Transform.position;
        r[4] = skeleton_R.Bones[(int)OVRSkeleton.BoneId.Hand_Pinky3].Transform.position;
    }

}
