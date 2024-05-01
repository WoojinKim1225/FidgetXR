using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class SetPlaneMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    [SerializeField] private Mesh mesh;

    public Vector2 size;

    public float curvature = 0.25f;
    public float resolution = 0.01f;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
    }

    void Update()
    {
        Debug.Log("Update...");
        if (size.x <= 0 || size.y <= 0) return;
        if (resolution <= 0) return;

        if (mesh == null) mesh = new Mesh();

        mesh = GenerateCurvedMesh();

        meshFilter.mesh = mesh;
        
    }

    private Mesh GenerateCurvedMesh() {
        float curvatureRadius = 1f / curvature;

        Vector3[] verts = new Vector3[(Mathf.CeilToInt(size.x / resolution) + 1) * 2];
        Vector2[] uvs = new Vector2[(Mathf.CeilToInt(size.x / resolution) + 1) * 2];
        int[] tris = new int[Mathf.CeilToInt(size.x / resolution) * 6];
        float maxAngle = (Mathf.CeilToInt(size.x / resolution) - 1) * resolution * curvature;

        for (int i = 0; i < Mathf.CeilToInt(size.x / resolution) + 1; i++) {
            float angle = i * resolution * curvature - maxAngle / 2f;
            verts[2 * i] = new Vector3(curvatureRadius * Mathf.Sin(angle), curvatureRadius * (1 - Mathf.Cos(angle)), size.y * (-0.5f));
            verts[2 * i + 1] = new Vector3(curvatureRadius * Mathf.Sin(angle), curvatureRadius * (1 - Mathf.Cos(angle)), size.y * 0.5f);
            uvs[2 * i] = Vector2.right * (i / (Mathf.CeilToInt(size.x / resolution) - 1.0f));
            uvs[2 * i + 1] = Vector2.right * (i / (Mathf.CeilToInt(size.x / resolution) - 1.0f)) + Vector2.up;
        }

        for (int i = 0; i < Mathf.CeilToInt(size.x / resolution) - 1; i++) {
            tris[i * 6] = 2 + 2 * i;
            tris[i * 6 + 1] = 2 * i;
            tris[i * 6 + 2] = 1 + 2 * i;
            tris[i * 6 + 3] = 1 + 2 * i;
            tris[i * 6 + 4] = 3 + 2 * i;
            tris[i * 6 + 5] = 2 + 2 * i;
        }

        mesh.Clear();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    
}