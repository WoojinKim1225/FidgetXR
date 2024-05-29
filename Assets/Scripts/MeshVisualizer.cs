using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MeshVisualizer : MonoBehaviour
{
    private MeshFilter meshFilter;
    [SerializeField] private Mesh mesh;

    public Vector2 size;
    private Vector2 _sizeOld;
    public float curvature = 0.25f;
    private float _curvatureOld;
    public float resolution = 0.01f;
    private float _resolutionOld;

    void Awake()
    {
        DestroyImmediate(mesh);
        meshFilter = GetComponent<MeshFilter>();
        InitMesh();
    }

    void Update()
    {
        if (size == _sizeOld && curvature == _curvatureOld && resolution == _resolutionOld) return;
        _sizeOld = size;
        _curvatureOld = curvature;
        _resolutionOld = resolution;


        Debug.Log("Update...");
        if (size.x <= 0 || size.y <= 0) return;
        if (resolution <= 0) return;

        InitMesh();

        if (curvature != 0f)
            mesh = GenerateCurvedMesh();
        else
            mesh = GenerateFlatMesh();

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
            verts[2 * i] = new Vector3(curvatureRadius * Mathf.Sin(angle), size.y * (-0.5f), -curvatureRadius * (1 - Mathf.Cos(angle)));
            verts[2 * i + 1] = new Vector3(curvatureRadius * Mathf.Sin(angle), size.y * 0.5f, -curvatureRadius * (1 - Mathf.Cos(angle)));
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

    private Mesh GenerateFlatMesh() {
        Vector3[] verts = new Vector3[4] {
            Vector3.left * size.x * 0.5f + Vector3.down * size.y * 0.5f, 
            Vector3.left * size.x * 0.5f + Vector3.up * size.y * 0.5f, 
            Vector3.right * size.x * 0.5f + Vector3.down * size.y * 0.5f, 
            Vector3.right * size.x * 0.5f + Vector3.up * size.y * 0.5f
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
        }
    }
}