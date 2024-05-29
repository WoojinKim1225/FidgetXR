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
        //XRDisplaySubsystem displaySubSystem = GetXRDisplaySubsystem();
        ///if (displaySubSystem == null || !displaySubSystem.running) return;

        //float ipd = GetIPD(out Vector3 leftEyePos, out Vector3 rightEyePos);

        //material.SetFloat("_IPD", ipd);
        //material.SetVector("_LeftEyePos", leftEyePos);
        //material.SetVector("_RightEyePos", rightEyePos);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Camera.main.stereoSeparation);
    }

    XRDisplaySubsystem GetXRDisplaySubsystem()
    {
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(displaySubsystems);
        if (displaySubsystems.Count > 0)
        {
            return displaySubsystems[0];
        }
        return null;
    }

    float GetIPD(out Vector3 left, out Vector3 right) {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        foreach (var device in devices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
            {
                if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePosition))
                {
                    if (device.TryGetFeatureValue(CommonUsages.leftEyePosition, out Vector3 leftEyePosition) &&
                        device.TryGetFeatureValue(CommonUsages.rightEyePosition, out Vector3 rightEyePosition))
                    {
                        left = leftEyePosition;
                        right = rightEyePosition;
                        return Vector3.Distance(leftEyePosition, rightEyePosition);
                    }
                }
            }
        }
        left = -Vector3.right * 0.032f;
        right = Vector3.right * 0.032f;
        return 0.064f; // 기본 IPD 값 (단위: 미터, 대략적인 평균값)
    }

}
