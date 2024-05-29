using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class IPDManager : MonoBehaviour
{
    public Material material;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_IPD", Camera.main.stereoSeparation); // 헤드셋을 안 꼈을 때는 0.022
    }

}
