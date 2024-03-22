using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FingerCurl
{
    public Vector3 rootPositionWS;
    public Vector3 rootDirectionWS;
    public Vector3 tipPositionWS;
    public Vector3 tipPositionBeforeWS;
    public Vector3 tipDirectionWS;
    public float curl;
    public Vector3 tipVelocityOS;
}
public class FingerCursorAuthoring : MonoBehaviour
{
    [SerializeField] private OVRSkeleton _leftHandSkeleton;
    [SerializeField] private OVRSkeleton _rightHandSkeleton;

    [SerializeField] private FingerCurl[] _fingerData;
    public FingerCurl[] FingerData => _fingerData;

    private void OnEnable() {
        _fingerData = new FingerCurl[10];
    }

    private void OnDisable() {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetFingerData(0, OVRSkeleton.BoneId.Hand_Thumb1, OVRSkeleton.BoneId.Hand_ThumbTip, Time.deltaTime);
    }

    void SetFingerData(int index, OVRSkeleton.BoneId rootId, OVRSkeleton.BoneId tipId, float dt) {
        _fingerData[index].rootPositionWS = _leftHandSkeleton.Bones[(int)rootId].Transform.position;
        _fingerData[index].rootDirectionWS = _leftHandSkeleton.Bones[(int)rootId].Transform.right;
        _fingerData[index].tipPositionWS = _leftHandSkeleton.Bones[(int)tipId].Transform.position;
        _fingerData[index].tipDirectionWS = _leftHandSkeleton.Bones[(int)tipId].Transform.right;
        _fingerData[index].curl = Vector3.Angle(_fingerData[index].rootDirectionWS, _fingerData[index].tipDirectionWS);
        _fingerData[index].tipVelocityOS = _leftHandSkeleton.Bones[(int)tipId].Transform.InverseTransformDirection((_fingerData[0].tipPositionWS - _fingerData[0].tipPositionBeforeWS) / dt);
        _fingerData[index].tipPositionBeforeWS = _fingerData[index].tipPositionWS;
    }

    private void OnDrawGizmos() {
        if (!_rightHandSkeleton.IsInitialized) return;
        float f = Vector3.Dot(_rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.right, _rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.right);
        Gizmos.color = new Color(f,0,0,1);
        Gizmos.DrawRay(_rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.position,_rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.right);
        Gizmos.DrawRay(_rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position,_rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.right);
        Gizmos.DrawLine(_rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.position, _rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position);
    }
}
