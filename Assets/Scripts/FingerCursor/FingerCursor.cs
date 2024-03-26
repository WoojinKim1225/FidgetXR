using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    [SerializeField] private uint connectFinger;
    public OVRSkeleton handSkeleton;
    public Vector3 bezierMidDirection;
    public float bezierMidDirectionAngle;

    //private List<PointData> ShapeDatas;
    [SerializeField] private List<GameObject> ShapeGameObjects;
    //[SerializeField] private List<Object> ShapeData;
    private int[] shapeDataConnectedEvent;
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
        ShapeGameObjects = new List<GameObject>() {null, null, null, null, null};
        //ShapeData = new List<Object>() {null, null, null, null, null};
        shapeDataConnectedEvent = new int[5] {-1, -1, -1, -1, -1};
    }

    private void OnDisable() {
        //ShapeData.Clear();
    }
    
    void Start()
    {
        waitTime = new WaitForSecondsRealtime(pinchClickTime);
    }

    void Update()
    {
        if (!handSkeleton.IsDataValid) return;
        tipPos[0] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position;
        tipPos[1] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        tipPos[2] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform.position;
        tipPos[3] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform.position;
        tipPos[4] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform.position;

        for (int i = 0; i < 5; i++) {
            bool isShown = hasBit(connectFinger, i);
            fingerLines[i].line.LineMeshRenderer.enabled = isShown;
            if (!isShown) continue;
            fingerLines[i].line.start.position = handSkeleton.Bones[(int)fingerLines[i].startBoneId].Transform.position;

            switch (ShapeGameObjects[i].tag) {
                case "Point":
                    PointData pointData = ShapeGameObjects[i].GetComponent<PointData>();
                    fingerLines[i].line.end.position = pointData.transform.TransformPoint(pointData.pointPositionOS[shapeDataConnectedEvent[i]]);

                    if (i != 0 && Vector3.Distance(tipPos[0], tipPos[i]) < pinchMargin.x && !hasBit(pointData.states, shapeDataConnectedEvent[i])) {
                        pointData.setStates(shapeDataConnectedEvent[i], 1);
                    }

                    if (i != 0 && Vector3.Distance(tipPos[0], tipPos[i]) > pinchMargin.y && hasBit(pointData.states, shapeDataConnectedEvent[i])) {
                        pointData.setStates(shapeDataConnectedEvent[i], 0);
                    }
                    break;
                case "Segment":
                    SegmentData segmentData = ShapeGameObjects[i].GetComponent<SegmentData>();
                    fingerLines[i].line.end.position = segmentData.transform.TransformPoint(Vector3.Lerp(segmentData.startPositionOS, segmentData.endPositionOS, segmentData.state01));

                    break;
                default:
                    break;
            }

            fingerLines[i].line.middleBezier.position = calculateMidBezierPos(fingerLines[i].line.start.position, fingerLines[i].line.end.position, handSkeleton.Bones[(int)fingerLines[i].startBoneId].Transform, bezierMidDirection, bezierMidDirectionAngle);

            
        }
    }
    
    private void FixedUpdate() {
        
    }

    void OnTriggerEnter(Collider other)
    {
        EConnectFinger otherFinger;
        int j;
        switch (other.tag) {
            case "Point":
                PointData otherPointData = other.GetComponent<PointData>();
                otherFinger = otherPointData.connectFinger;
                j = 0;
                for (int i = 0; i < 5; i++) {
                    if (!hasBit((uint)otherFinger, i)) continue;
                    if (!hasBit(connectFinger,i)) {
                        ShapeGameObjects[i] = other.gameObject;
                        shapeDataConnectedEvent[i] = j;
                        connectFinger = setBit(connectFinger, i, 1);
                        otherPointData.connectedFinger = setBit(otherPointData.connectedFinger, i, 1);
                    }
                    j++;
                }
                break;
            case "Segment":
                SegmentData otherSegmentData = other.GetComponent<SegmentData>();
                otherFinger = otherSegmentData.connectFinger;
                j = 0;
                for (int i = 0; i < 5; i++) {
                    if (!hasBit((uint)otherFinger, i)) continue;
                    if (!hasBit(connectFinger,i)) {
                        ShapeGameObjects[i] = other.gameObject;
                        shapeDataConnectedEvent[i] = j;
                        connectFinger = setBit(connectFinger, i, 1);
                        otherSegmentData.connectedFinger = setBit(otherSegmentData.connectedFinger, i, 1);
                    }
                    j++;
                }
                break;
            default:
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        EConnectFinger otherFinger;
        int j;
        switch (other.tag) {
            case "Point":
                PointData otherPointData = other.GetComponent<PointData>();
                otherFinger = otherPointData.connectFinger;
                j = 0;
                for (int i = 0; i < 5; i++) {
                    if (!hasBit((uint)otherFinger, i)) continue;
                    if (hasBit(connectFinger,i) && ShapeGameObjects[i] == other.gameObject) {
                        ShapeGameObjects[i] = null;
                        shapeDataConnectedEvent[i] = -1;
                        connectFinger = setBit(connectFinger, i, 0);
                        otherPointData.connectedFinger = setBit(otherPointData.connectedFinger, i, 0);
                    }
                    j++;
                }
                break;
            case "Segment":
                SegmentData otherSegmentData = other.GetComponent<SegmentData>();
                otherFinger = otherSegmentData.connectFinger;
                j = 0;
                for (int i = 0; i < 5; i++) {
                    if (!hasBit((uint)otherFinger, i)) continue;
                    if (hasBit(connectFinger,i) && ShapeGameObjects[i] == other.gameObject) {
                        ShapeGameObjects[i] = null;
                        shapeDataConnectedEvent[i] = -1;
                        connectFinger = setBit(connectFinger, i, 0);
                        otherSegmentData.connectedFinger = setBit(otherSegmentData.connectedFinger, i, 0);
                    }
                    j++;
                }
                break;
            default:
                break;
        }
    }
    
    public Vector3 calculateMidBezierPos(Vector3 a, Vector3 b, Transform start, Vector3 direction, float angle) {
        Vector3 d = b - a;
        Vector3 d1 = Vector3.Project(d, start.TransformDirection(direction).normalized);
        Vector3 d2 = d - d1;
        if (Vector3.Angle(d1, d) > angle) d2 = Vector3.Normalize(d2) * Vector3.Magnitude(d1) * Mathf.Tan(angle * Mathf.Deg2Rad);
        if (Vector3.Dot(d1, start.TransformDirection(direction)) < 0) d1 *= -1;
        return start.position + ((d1 + d2) * 0.5f);
    }

    public bool hasBit(uint source, int bit) {
        return ((source >> bit) & 1) == 1;
    }

    public uint setBit(uint source, int bit, int value) {
        if (value == 0) {
            return source & ~(1u << bit);
        } else {
            return source | (1u << bit);
        }
    }
}
