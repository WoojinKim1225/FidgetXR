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
    public float bezierMidDirectionAngle;

    private Object[] ShapeDatas;
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
        ShapeDatas = new PointData[5];
    }
    
    void Start()
    {
        waitTime = new WaitForSecondsRealtime(pinchClickTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!handSkeleton.IsDataValid) return;
        tipPos[0] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position;
        tipPos[1] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        tipPos[2] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform.position;
        tipPos[3] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform.position;
        tipPos[4] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform.position;

        /*
        for (int i = 0; i < 5; i++) {
            bool isShown = ((interactableFinger >> i) & 1) == 1;
            fingerLines[i].line.LineMeshRenderer.enabled = isShown;
            if (i == 0) continue;
            if (isShown && Vector3.Distance(tipPos[0], tipPos[i]) < pinchMargin.x && !fingerLines[i].isPressed) {
                fingerLines[i].isPressed = true;
                Debug.Log("Pressed!: " + i);
                StartCoroutine(IPinch(i));
            }
        }
        */

    }
    /*
    void OnTriggerStay(Collider other)
    {   
        if (other.tag == "Point") {
            for (int i = 0; i < 5; i++) {
                if (ShapeDatas[i] == null) ShapeDatas[i] = other.GetComponent<PointData>();
                interactableFinger = (uint)pointData.InteractableFinger;
                fingerLines[i].line.start.position = handSkeleton.Bones[(int)fingerLines[i].startBoneId].Transform.position;
                fingerLines[i].line.end.position = other.transform.position;

                fingerLines[i].line.middleBezier.position = calculateMidBezierPos(fingerLines[i].line.start.position, fingerLines[i].line.end.position, handSkeleton.Bones[(int)fingerLines[i].startBoneId].Transform, bezierMidDirection, bezierMidDirectionAngle);
            }
            return;
        }
        
    }
    */

    void OnTriggerStay(Collider other) {
        
    }

    void OnTriggerExit(Collider other)
    {
        interactableFinger = 0;
    }

    /*
    public IEnumerator IPinch(int i) {
        pointData.unityEventButtons[i-1].OnPress.Invoke();
        yield return waitTime;
        if (Vector3.Distance(tipPos[0], tipPos[i]) > pinchMargin.y) {
            pointData.unityEventButtons[i-1].OnClick.Invoke();
            fingerLines[i].isPressed = false;
            yield break;
        }
        yield return new WaitUntil(() => Vector3.Distance(tipPos[0], tipPos[i]) > pinchMargin.y);
        pointData.unityEventButtons[i-1].OnRelease.Invoke();
        fingerLines[i].isPressed = false;
        yield break;
    }
    */

    public Vector3 calculateMidBezierPos(Vector3 a, Vector3 b, Transform start, Vector3 direction, float angle) {
        Vector3 d = b - a;
        Vector3 d1 = Vector3.Project(d, start.TransformDirection(direction).normalized);
        Vector3 d2 = d - d1;
        if (Vector3.Angle(d1, d) > angle) d2 = Vector3.Normalize(d2) * Vector3.Magnitude(d1) * Mathf.Tan(angle * Mathf.Deg2Rad);
        if (Vector3.Dot(d1, start.TransformDirection(direction)) < 0) d1 *= -1;
        return start.position + ((d1 + d2) * 0.5f);
    }
}
