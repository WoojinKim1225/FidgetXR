using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MeshVisualizer : MonoBehaviour
{
    private MeshFilter meshFilter;
    [SerializeField] private Mesh mesh;

    public Stateful<Vector2> size;
    public Stateful<float> curvature;
    public Stateful<float> resolution;

    void Awake()
    {
        Destroy(mesh);
        meshFilter = GetComponent<MeshFilter>();
        size = new Stateful<Vector2>(Vector2.zero);
        curvature = new Stateful<float>(0);
        resolution = new Stateful<float>(1);
        InitMesh();
    }

    void Update()
    {
        if (size.Value.x <= 0 || size.Value.y <= 0) return;
        if (resolution.Value <= 0) return;

        if (!size.IsChanged && !curvature.IsChanged && !resolution.IsChanged) return;

        InitMesh();

        if (curvature.Value != 0f)
            mesh = GenerateCurvedMesh();
        else
            mesh = GenerateFlatMesh();

        meshFilter.mesh = mesh;
        
    }

    private Mesh GenerateCurvedMesh() {
        float curvatureRadius = 1f / curvature.Value;

        Vector3[] verts = new Vector3[(Mathf.CeilToInt(size.Value.x / resolution.Value) + 1) * 2];
        Vector2[] uvs = new Vector2[(Mathf.CeilToInt(size.Value.x / resolution.Value) + 1) * 2];
        int[] tris = new int[Mathf.CeilToInt(size.Value.x / resolution.Value) * 6];
        float maxAngle = (Mathf.CeilToInt(size.Value.x / resolution.Value) - 1) * resolution.Value * curvature.Value;

        for (int i = 0; i < Mathf.CeilToInt(size.Value.x / resolution.Value) + 1; i++) {
            float angle = i * resolution.Value * curvature.Value - maxAngle / 2f;
            verts[2 * i] = new Vector3(curvatureRadius * Mathf.Sin(angle), size.Value.y * (-0.5f), -curvatureRadius * (1 - Mathf.Cos(angle)));
            verts[2 * i + 1] = new Vector3(curvatureRadius * Mathf.Sin(angle), size.Value.y * 0.5f, -curvatureRadius * (1 - Mathf.Cos(angle)));
            uvs[2 * i] = Vector2.right * (i / (Mathf.CeilToInt(size.Value.x / resolution.Value) - 1.0f));
            uvs[2 * i + 1] = Vector2.right * (i / (Mathf.CeilToInt(size.Value.x / resolution.Value) - 1.0f)) + Vector2.up;
        }

        for (int i = 0; i < Mathf.CeilToInt(size.Value.x / resolution.Value) - 1; i++) {
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

    private Mesh GenerateFlatMesh() {
        Vector3[] verts = new Vector3[4] {
            Vector3.left * size.Value.x * 0.5f + Vector3.down * size.Value.y * 0.5f, 
            Vector3.left * size.Value.x * 0.5f + Vector3.up * size.Value.y * 0.5f, 
            Vector3.right * size.Value.x * 0.5f + Vector3.down * size.Value.y * 0.5f, 
            Vector3.right * size.Value.x * 0.5f + Vector3.up * size.Value.y * 0.5f
        };
        Vector2[] uvs = new Vector2[4] {
            Vector2.zero,
            Vector2.up,
            Vector2.right,
            Vector2.right + Vector2.up
        };
        int[] tris = new int[6] {
            2, 0, 1, 1, 3, 2
        };

        mesh.Clear();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
        
    }

    private void InitMesh() {
        if (mesh == null) {
            mesh = new Mesh
            {
                name = "meshPlane"
            };
        } else {
            mesh.name = "meshPlane";
        }
    }
}