using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteInEditMode]
public class ArcMesh : MonoBehaviour
{
    public Mesh mesh;
    public MeshFilter meshFilter;
    public Arc arc;
    //public bool isUpdate;

    private List<Vector3> vertices;
    private List<int> triangles;
    List<Vector2> uv;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void OnUpdate()
    {
        float radius = arc.Radius - arc.arcVisual.startWidth / 2;
        vertices = new List<Vector3>
        {
            // Add the first vertex at the origin
            Vector3.zero
        };

        float angleStep = Mathf.Abs(arc.EndAngle - arc.StartAngle) / arc.ArcSegmentCount;
        for (int i = 0; i <= arc.ArcSegmentCount; i++)
        {
            float angleRad = Mathf.Deg2Rad * (arc.StartAngle + angleStep * i);
            vertices.Add(radius * new Vector3(Mathf.Sin(angleRad), Mathf.Cos(angleRad), 0));
        }

        triangles = new List<int>();

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);         // Index of the center vertex
            triangles.Add(i);         // Current vertex index
            triangles.Add(i + 1);     // Next vertex index
        }

        uv = new List<Vector2>();

        for (int i = 0; i < vertices.Count; i++)
        {
            float u = vertices[i].x / (2 * radius) + 0.5f;
            float v = vertices[i].y / (2 * radius) + 0.5f;
            uv.Add(new Vector2(u, v));
        }

        mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uv.ToArray()   // Optional
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

}
