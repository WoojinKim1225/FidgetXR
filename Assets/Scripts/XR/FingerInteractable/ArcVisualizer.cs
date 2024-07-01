using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteInEditMode]
public class ArcVisualizer : MonoBehaviour
{
    public Mesh mesh;
    public Arc arc;

    void Update()
    {
        //Vector3[] verticies = new Vector3[arc.arcSegmentCount]
    }

}
