using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshInteractable : MonoBehaviour
{
    public MeshFilter meshVisualizer;
    private Mesh mesh;

    public Vector2 size;

    public enum EMeshType {
        Flat, Curved, Spherical
    }
    public EMeshType meshType = EMeshType.Flat;

    public float curvature = 0.25f;
    public float resolution = 0.01f;

    
    
    Vector3 GetClosestPointOS(Vector3 position) {
        return Vector3.zero;
    }
    

    private void Start() {
        
    }
    
    
    
}
