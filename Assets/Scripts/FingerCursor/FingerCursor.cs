using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void D();

public struct ButtonDelegate {
    public D onPress, onRelease, onClick;
}
[System.Serializable]
public struct FingerAction {
    public ButtonDelegate[] pinches; //pI, pM, pR, pP;
    public ButtonDelegate[] curls; //dT, dI, dM, dR, dP;
    public ButtonDelegate[] bends; //bT, bI, bM, bR, bP;
    public ButtonDelegate[] claws; //cT, cI, cM, cR, cP;
}

public class FingerCursor : MonoBehaviour
{
    [SerializeField] private OVRSkeleton hand_L, hand_R;
    public FingerAction action_L, action_R;

    public Vector3[] fingerTips_L, fingerTips_R;

    public int[] boneTipId = {(int)OVRSkeleton.BoneId.Hand_ThumbTip, (int)OVRSkeleton.BoneId.Hand_IndexTip, (int)OVRSkeleton.BoneId.Hand_MiddleTip, (int)OVRSkeleton.BoneId.Hand_RingTip, (int)OVRSkeleton.BoneId.Hand_PinkyTip};

    void OnEnable()
    {
        fingerTips_L = new Vector3[5];
        fingerTips_R = new Vector3[5];

        action_L.pinches = new ButtonDelegate[4];
        action_L.curls = new ButtonDelegate[5];
        action_L.bends = new ButtonDelegate[5];
        action_L.claws = new ButtonDelegate[5];

        action_R.pinches = new ButtonDelegate[4];
        action_R.curls = new ButtonDelegate[5];
        action_R.bends = new ButtonDelegate[5];
        action_R.claws = new ButtonDelegate[5];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++) {
            fingerTips_L[i] = hand_L.Bones[boneTipId[i]].Transform.position;
            fingerTips_R[i] = hand_R.Bones[boneTipId[i]].Transform.position;
        }

        
    }
}
