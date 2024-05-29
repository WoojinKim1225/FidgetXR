using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICursorMapper : MonoBehaviour
{
    public MeshVisualizer meshVisualizer;
    public MeshInteractable meshInteractable;
    UIDocument document;

    VisualElement cursor;

    private Vector2 pixelUV;
    private Vector2 dampedPixelUV;

    public float c;


    void Awake()
    {
        meshVisualizer = GetComponent<MeshVisualizer>();
    }
    
    private void OnEnable() {
        document = GetComponent<UIDocument>();
        cursor = document.rootVisualElement.Q<VisualElement>("cursor");
    }

    void LateUpdate()
    {
        OnCursorUpdate();
    }

    public void OnCursorUpdate() {
        Vector3 originOS = transform.InverseTransformPoint(meshInteractable.pointerOrigin);
        Vector3 directionOS = transform.InverseTransformDirection(meshInteractable.pointerDirection + transform.forward * 0.3f).normalized;

        if (originOS.z * directionOS.z < 0) {
            Vector3 hitOS = originOS - directionOS * originOS.z / directionOS.z;
            Vector3 documentSize = new Vector2(document.panelSettings.targetTexture.width, document.panelSettings.targetTexture.height);
            
            Vector2 uv = new Vector2(hitOS.x, hitOS.y) / meshVisualizer.size.Value + Vector2.one * 0.5f;

            pixelUV = (Vector2.up + uv * new Vector2(1f, -1f)) * documentSize;

            // pixelUV.x = Mathf.Clamp01(pixelUV.x) * document.panelSettings.targetTexture.width;
            // pixelUV.y = Mathf.Clamp01(1 - pixelUV.y) * document.panelSettings.targetTexture.height;

            //dampedPixelUV = dampedHarmonic(pixelUV, c, dampedPixelUV);

            if (cursor != null) {
                cursor.visible = true;
                cursor.style.left = pixelUV.x;
                cursor.style.top = pixelUV.y;
            }
        } else {
            if (cursor != null) {
                cursor.visible = false;
            }
        }
    }

    public void SetCursorVisible(bool b) {
        cursor.visible = b;
    }

}