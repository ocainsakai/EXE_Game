
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    private TouchControl touchControl;
    public delegate void TouchEvent(Vector2 position, float time);
    public event TouchEvent OnStartTouch;
    public event TouchEvent OnEndTouch;
    public event TouchEvent OnTouch;
    protected override void Awake()
    {
        base.Awake();
        touchControl = new TouchControl();
    }

    private void OnEnable()
    {
        touchControl.Enable();
        TouchSimulation.Enable();
    }

  

    private void OnDisable()
    {
        touchControl.Disable();
        TouchSimulation.Disable();

    }

    private void Start()
    {
        touchControl.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControl.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
        touchControl.Touch.TouchPress.performed += ctx => OnTouchMove(ctx);
    }
    private void OnTouchMove(InputAction.CallbackContext ctx)
    {
        OnTouch?.Invoke(touchControl.Touch.TouchPosition.ReadValue<Vector2>(), Time.time);
    }
    private void EndTouch(InputAction.CallbackContext ctx)
    {
        OnEndTouch?.Invoke(touchControl.Touch.TouchPosition.ReadValue<Vector2>(), Time.time);
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        OnStartTouch?.Invoke(touchControl.Touch.TouchPosition.ReadValue<Vector2>(), Time.time);
    }
}
