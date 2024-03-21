using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class FingerCursorAuthoring : MonoBehaviour
{

    public class FingerCursorBaker : Baker<FingerCursorAuthoring>
    {
        public override void Bake(FingerCursorAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            AddComponent(entity, new FingerData{
                Axis = math.forward(),
                Angle = 135f
            });
            AddComponent(entity, new FingerPointData{
                pointEntity = Entity.Null,
                Transform = float4x4.identity
            });
        }
    }   
}
