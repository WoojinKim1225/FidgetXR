using UnityEngine;
using UnityEngine.Events;

public class FingerData : MonoBehaviour
{
    [System.Flags] public enum EFingerNumber{None = 0, Thumb = 1, Index = 2, Middle = 4, Ring = 8, Pinky = 16}

    public EFingerNumber fingerNumber;

    public Transform proximal, distal, thumbTip;

    public FingerInteractable controllingObject;
    public FloatButton curlAmount = new FloatButton(0, 0.2f);
    public FloatButton touchAmount = new FloatButton(0, 0.2f);
    public FloatButton pinchAmount = new FloatButton(0, 0.2f);
    public FloatButton tapAmount = new FloatButton(0, 0.2f);

    public Vector2 curlThreshold;
    public Vector2 pinchThreshold;
    public Vector2 touchThreshold;
    public Vector2 tapThreshold;


    void Update()
    {
        float dt = Time.deltaTime;
        curlAmount.OnUpdate(Vector3.Angle(proximal.forward, distal.forward), curlThreshold, 1f, dt);
        if (transform.parent == thumbTip) {
            pinchAmount.OnUpdate(-1f, pinchThreshold, -1f, dt);
            tapAmount.OnUpdate(-1f, tapThreshold, -1f, dt);
        } else {
            pinchAmount.OnUpdate(Vector3.Distance(transform.position, thumbTip.position), pinchThreshold, -1f, dt);
            tapAmount.OnUpdate(Vector3.Distance(proximal.position, thumbTip.position), tapThreshold, -1f, dt);
        }
        if (controllingObject == null) {
            touchAmount.OnUpdate(-1, touchThreshold, -1f, dt);
            return;
        }
        touchAmount.OnUpdate(controllingObject.DistanceFunction(transform), touchThreshold, -1f, dt);

        //------------------

        FingerState2Event(curlAmount, controllingObject.fingerActions.curl);
        FingerState2Event(pinchAmount, controllingObject.fingerActions.pinch);
        FingerState2Event(touchAmount, controllingObject.fingerActions.touch);
        FingerState2Event(tapAmount, controllingObject.fingerActions.tap);
    }


    void OnTriggerEnter(Collider other)
    {
        FingerInteractable interactable = other.GetComponent<FingerInteractable>();
        if (((int)interactable.fingerActions.fingerMask & (int)fingerNumber) == 0) return;
        controllingObject = interactable;
        controllingObject.fingerActions.hover.OnEnter.Invoke();
    }

    void OnTriggerStay(Collider other) {
        controllingObject.fingerActions.hover.OnStay.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        controllingObject.fingerActions.hover.OnExit.Invoke();
        controllingObject = null;
    }

    private void FingerState2Event(FloatButton Value, UnityEventStateful stateful) {
        switch (Value.state) {
            case EButtonState.Press:
                stateful.OnEnter.Invoke();
                break;
            case EButtonState.Hold:
                stateful.OnStay.Invoke();
                break;
            case EButtonState.Pull:
                stateful.OnExit.Invoke();
                break;
            case EButtonState.None:
                break;
            case EButtonState.Click:
                stateful.OnClick.Invoke();
                break;
            default:
                break;
        }
    }
}
