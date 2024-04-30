using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInteractable : MonoBehaviour
{
    public MeshRenderer meshVisualizer;
    private Mesh mesh;

    public Vector2 size;

    public enum EMeshType {
        Flat, Curved, Spherical
    }
    public EMeshType meshType = EMeshType.Flat;

    public float curvature = 0.25f;
    public float resolution = 0.01f;

    /*
    
    Vector3 GetClosestPointOS(Vector3 position) {

    }
    */

    private void Start() {
        //mesh = GenerateCurvedMesh();
    }
    /*
    private Mesh GenerateCurvedMesh() {
        mesh = new Mesh();
        float curvatureRadius = 1f / curvature;

        Vector3[] verts = new Vector3[(Mathf.CeilToInt(size.x / resolution) + 1) * 2];
        int[] tris = new int[Mathf.CeilToInt(size.x / resolution) * 6];
        for (int i = 0; i < verts.Length / 2; i++) {
            float angle = i * resolution * curvature;
            verts[2 * i] = new Vector3(curvatureRadius * Mathf.Sin(angle), curvatureRadius * (1 - Mathf.Cos(angle)), size.y * (-0.5f));
            verts[2 * i + 1] = new Vector3(curvatureRadius * Mathf.Sin(angle), curvatureRadius * (1 - Mathf.Cos(angle)), size.y * 0.5f);
        }
        for (int i = 0; i < Mathf.CeilToInt(size.x / resolution); i++) {

        }
    }
    */
}
