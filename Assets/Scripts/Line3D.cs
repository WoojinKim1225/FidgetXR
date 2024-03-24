using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line3D : MonoBehaviour
{
    public SkinnedMeshRenderer LineMeshRenderer;
    public Transform start, end;
    public Transform middleBezier;
    public List<Transform> Vertices;
    private int _vertCount;

    private Vector3 _startPosWS, _midPosWS, _endPosWS;

    void Awake()
    {
        LineMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void OnEnable()
    {
        Vertices.Clear();
        Transform bone = LineMeshRenderer.rootBone;
        start = bone.transform;
        Vertices.Add(bone);
        while(bone.childCount == 1) {
            bone = bone.GetChild(0);
            Vertices.Add(bone);
        }
        end = bone.transform;
        _vertCount = Vertices.Count;
    }
    
    void OnDisable()
    {
        Vertices.Clear();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        _startPosWS = start.position;
        _endPosWS = end.position;
        _midPosWS = middleBezier.position;

        for (int i = 0; i < _vertCount; i++) {
            float t = i / (float)(_vertCount - 1);
            float a = (1 - t) * (1 - t);
            float b = 2 * (1 - t) *  t;
            float c = t * t;
            Vertices[i].position = _startPosWS * a + _midPosWS * b + _endPosWS * c;
            Vertices[i].up = 2 * (1 - t) * (_midPosWS - _startPosWS) + 2 * t * (_endPosWS - _midPosWS);
        }
    }
}
