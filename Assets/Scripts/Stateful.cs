using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventStateful
{
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;
    public UnityEvent OnClick;
}

[System.Serializable]
public class Float{
    public float value;
    public float beforeValue;
    public bool isChanged;
    public float initialValue;

    public Float(float val) {
        this.value = val;
        this.beforeValue = val;
        this.isChanged = false;
        this.initialValue = val;
    }

    public void OnUpdate(float val) {
        this.beforeValue = this.value;
        this.value = val;
        this.isChanged = this.beforeValue != val;
    }

    public void OnUpdate() {
        this.isChanged = this.beforeValue != this.value;
        this.beforeValue = this.value;
    }
}
public enum EButtonState{
    None = 0, Press = 1, Pull = 2, Hold = 3, Click = 4
}

[System.Serializable]
public class FloatButton{
    public float value;
    public float beforeValue;
    public bool isChanged;
    public float initialValue;
    public EButtonState state;

    public Vector2 timer;

    public FloatButton(float val, float timerInitial) {
        value = val;
        beforeValue = val;
        isChanged = false;
        initialValue = val;
        state = 0;
        timer = new Vector2(timerInitial, 0);
    }

    public void OnUpdate(float val, Vector2 threshold, float direction, float dt) {
        if (threshold.x * direction <= threshold.y * direction) {
            Debug.LogError("The Threshold of " + ToString() + "'s in value and out value is invalid!");
            return;
        }
        if (timer.y > 0) timer.y -= dt;
        beforeValue = value;
        value = val;
        isChanged = beforeValue == val;
        if (value * direction < threshold.x * direction && state == EButtonState.Pull) {
            state = EButtonState.None;
        }
        if (value * direction < threshold.y * direction && state == EButtonState.Hold) {
            if (timer.y > 0) {
                timer.y = 0;
                state = EButtonState.Click;
            } else state = EButtonState.Pull;
        }
        if (value * direction > threshold.y * direction && state == EButtonState.Press) {
            state = EButtonState.Hold;
        }
        if (value * direction > threshold.x * direction && state == EButtonState.None) {
            state = EButtonState.Press;
            timer.y = timer.x;
        }
    }
}