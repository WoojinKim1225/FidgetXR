using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SDFManager : MonoBehaviour
{
    public Material SDFMaterial;
    public Vector3 positionWS;
    public float radius;

    public Transform thumbTransform, indexTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        positionWS = (thumbTransform.position + indexTransform.position) / 2f;
        radius = Mathf.Min(Vector3.Distance(thumbTransform.position, indexTransform.position) / 2f - 0.01f, 0.02f);
        radius = Mathf.Max(radius, 0.002f);
        SDFMaterial.SetVector("_Position", positionWS);
        SDFMaterial.SetFloat("_Radius", radius);
    }
}
