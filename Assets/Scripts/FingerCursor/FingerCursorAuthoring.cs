using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerCursorAuthoring : MonoBehaviour
{
    public OVRSkeleton rightHandSkeleton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rightHandSkeleton.IsInitialized) return;
        float f = Vector3.Dot(rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.forward, rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.forward);
        Debug.DrawRay(rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.position,rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.forward);
        Debug.DrawRay(rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index3].Transform.position,rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index3].Transform.forward);
        Debug.DrawLine(rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.position, rightHandSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position, new Color(f,0,0,1));
    }
}
