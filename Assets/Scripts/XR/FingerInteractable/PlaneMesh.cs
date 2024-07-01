using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(UIDocument))]
[ExecuteInEditMode]
public class PlaneMesh : MonoBehaviour
{
    public Mesh mesh;
    public MeshFilter meshFilter;
    public Plane plane;
    public UIDocument document;

    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uv;

    public bool b;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        document = GetComponent<UIDocument>();
        triangles = new List<int> {0, 1, 2, 2, 1, 3};
        uv = new List<Vector2> {
            Vector2.zero, Vector2.up, Vector2.right, Vector2.one
        }; 
    }

    public void OnUpdate()
    {
        vertices = new List<Vector3>{
            - plane.size.x * 0.5f * Vector3.right - plane.size.y * 0.5f * Vector3.up,
            - plane.size.x * 0.5f * Vector3.right + plane.size.y * 0.5f * Vector3.up,
            plane.size.x * 0.5f * Vector3.right - plane.size.y * 0.5f * Vector3.up,
            plane.size.x * 0.5f * Vector3.right + plane.size.y * 0.5f * Vector3.up,
        };

        mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uv.ToArray()
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

    public void SetCursor(Vector2 uv) {
        var cursor = document.rootVisualElement.Q<VisualElement>("cursor");
        cursor.style.left = uv.x;
        cursor.style.top = uv.y;
    }
}