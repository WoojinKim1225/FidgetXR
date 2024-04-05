using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Unity.Mathematics;

namespace Finger
{
    public enum HandType
    {
        Null = 0, Left = 1, Right = 2
    }

    public class FingerEntityManager : MonoBehaviour
    {
        public bool b;
        public HandType handType;
        public HandVisual handVisual;

        public HandJointId[] handTipJointIds = { HandJointId.HandThumbTip, HandJointId.HandIndexTip, HandJointId.HandMiddleTip, HandJointId.HandRingTip, HandJointId.HandPinkyTip };
        public HandJointId[] handRootJointIds = { HandJointId.HandThumb1, HandJointId.HandIndex1, HandJointId.HandMiddle1, HandJointId.HandRing1, HandJointId.HandPinky1 };
        private Entity thumbEntity;

        // Start is called before the first frame update
        void Start()
        {
            handVisual = GetComponent<HandVisual>();
        }

        // Update is called once per frame
        void Update()
        {
            if (b) return;
            b = true;
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            for (int i = 0; i < 5; i++)
            {
                Entity entity = entityManager.CreateEntity(
                    typeof(LocalToWorld),
                    typeof(LocalTransform),
                    typeof(ConnectedFingerReference),
                    typeof(HandData),
                    typeof(ConnectedThumbEntity),
                    typeof(ConnectedPalmEntity),
                    typeof(ConnectedFingerTransform),
                    typeof(FingerAction));

                if (i == 0) thumbEntity = entity;
                else
                {
                    entityManager.SetComponentData(entity, new ConnectedThumbEntity
                    {
                        Thumb = thumbEntity
                    });
                }

                entityManager.SetComponentData(entity, new ConnectedFingerReference
                {
                    TipGameObject = handVisual.GetTransformByHandJointId(handTipJointIds[i]).gameObject,
                    RootGameObject = handVisual.GetTransformByHandJointId(handRootJointIds[i]).gameObject,
                    ThumbTipGameObject = handVisual.GetTransformByHandJointId(HandJointId.HandThumbTip).gameObject,
                    PalmGameObject = handVisual.GetTransformByHandJointId(HandJointId.HandWristRoot).gameObject
                });

                entityManager.SetComponentData(entity, new HandData
                {
                    HandNumber = (int)handType
                });

                entityManager.SetComponentData(entity, new ConnectedFingerTransform
                {
                    frontAxis = handType == HandType.Left ? math.left() : math.right(),
                    upAxis = handType == HandType.Right ? math.down() : math.up(),

                });

                entityManager.SetComponentData(entity, new FingerAction
                {
                    CurlEnableMargin = -0.5f,
                    CurlDisableMargin = -0.3f,
                    BendEnableMargin = 0.5f,
                    BendDisableMargin = 0.3f,
                    PinchEnableMargin = 0.005f,
                    PinchDisableMargin = 0.008f
                });
            }
        }
    }
}