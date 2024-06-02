using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class IPDManager : MonoBehaviour
{
    public Material material;
    public float ipd;
    // Start is called before the first frame update
    void Start()
    {
        ipd = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Camera.main.stereoSeparation - ipd) < 0.001f) return;
        ipd = Camera.main.stereoSeparation;
        material.SetFloat("_IPD", ipd); // 헤드셋을 안 꼈을 때는 0.022
    }

}
