using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FingerLine {
    public OVRSkeleton.BoneId startBoneId;
    public Vector3 mid;
    public Vector3 end;
    public GameObject InstantiatedLine;
    public Line3D line;
    public bool isPressed;
}

public class FingerCursor : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    private FingerLine[] fingerLines;
    public uint interactableFinger;
    public OVRSkeleton handSkeleton;
    public Vector3 bezierMidDirection;

    private PointData pointData;
    private Vector3[] tipPos;

    public Vector2 pinchMargin;
    public float pinchClickTime;
    private WaitForSecondsRealtime waitTime;
    
    void OnEnable()
    {
        handSkeleton = transform.parent.GetComponent<OVRSkeleton>();
        fingerLines = new FingerLine[5];
        tipPos = new Vector3[5];
        fingerLines[0].startBoneId = OVRSkeleton.BoneId.Hand_ThumbTip;
        fingerLines[1].startBoneId = OVRSkeleton.BoneId.Hand_IndexTip;
        fingerLines[2].startBoneId = OVRSkeleton.BoneId.Hand_MiddleTip;
        fingerLines[3].startBoneId = OVRSkeleton.BoneId.Hand_RingTip;
        fingerLines[4].startBoneId = OVRSkeleton.BoneId.Hand_PinkyTip;
        for (int i = 0; i < 5; i++) {
            fingerLines[i].InstantiatedLine = Instantiate(linePrefab);
            fingerLines[i].line = fingerLines[i].InstantiatedLine.GetComponent<Line3D>();
        }
        pointData = null;
    }
    
    void Start()
    {
        waitTime = new WaitForSecondsRealtime(pinchClickTime);
    }

    // Update is called once per frame
    void Update()
    {
        tipPos[0] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position;
        tipPos[1] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        tipPos[2] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform.position;
        tipPos[3] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform.position;
        tipPos[4] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform.position;

        for (int i = 0; i < 5; i++) {
            bool isShown = ((interactableFinger >> i) & 1) == 1;
            fingerLines[i].line.LineMeshRenderer.enabled = isShown;
            if (i == 0) continue;
            if (isShown && Vector3.Distance(tipPos[0], tipPos[i]) < pinchMargin.x && !fingerLines[i].isPressed) {
                fingerLines[i].isPressed = true;
                StartCoroutine(IPinch(i));
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Point") {
            if (pointData == null) pointData = other.GetComponent<PointData>();
            interactableFinger = (uint)pointData.InteractableFinger;
            for (int i = 0; i < 5; i++) {
                fingerLines[i].line.start.position = handSkeleton.Bones[(int)fingerLines[i].startBoneId].Transform.position;
                fingerLines[i].line.end.position = other.transform.position;
                float distance = (fingerLines[i].line.start.position - fingerLines[i].line.end.position).magnitude;
                fingerLines[i].line.middleBezier.position = handSkeleton.Bones[(int)fingerLines[i].startBoneId].Transform.TransformPoint(bezierMidDirection * distance * 0.5f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        interactableFinger = 0;
    }

    public IEnumerator IPinch(int i) {
        pointData.unityEventButtons[i-1].OnPress.Invoke();
        yield return waitTime;
        if (Vector3.Distance(tipPos[0], tipPos[i]) > pinchMargin.y) {
            pointData.unityEventButtons[i-1].OnClick.Invoke();
            yield break;
        }
        yield return new WaitUntil(() => Vector3.Distance(tipPos[0], tipPos[i]) > pinchMargin.y);
        pointData.unityEventButtons[i-1].OnRelease.Invoke();
        yield break;
    }
}
