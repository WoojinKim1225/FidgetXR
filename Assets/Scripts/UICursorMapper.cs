using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICursorMapper : MonoBehaviour
{
    public PlaneMeshManager planeMeshManager;
    public MeshInteractable meshInteractable;
    UIDocument document;

    VisualElement cursor;

    private Vector2 pixelUV;
    private Vector2 dampedPixelUV;

    public float c;


    void Awake()
    {
        planeMeshManager = GetComponent<PlaneMeshManager>();
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
            
            pixelUV = (new Vector2(hitOS.x, hitOS.y) + planeMeshManager.size * 0.5f) / planeMeshManager.size;
            pixelUV.x = Mathf.Clamp01(pixelUV.x) * document.panelSettings.targetTexture.width;
            pixelUV.y = Mathf.Clamp01(1 - pixelUV.y) * document.panelSettings.targetTexture.height;

            dampedPixelUV = dampedHarmonic(pixelUV, c, dampedPixelUV);


            if (cursor != null) {
                cursor.visible = true;
                cursor.style.left = dampedPixelUV.x;
                cursor.style.top = dampedPixelUV.y;
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

    private Vector2 dampedHarmonic(Vector2 target, float c, Vector2 pos)
    {
        // Calculate the difference between the current position and the target position
        Vector2 difference = target - pos;

        // Apply damping
        Vector2 dampedDifference = difference * Mathf.Exp(-c * Time.deltaTime);

        // Update position
        Vector2 newPos = pos + dampedDifference;

        return newPos;
    }
}