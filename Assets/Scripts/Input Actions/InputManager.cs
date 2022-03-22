using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, InputActions.IMouseActions
{
    public delegate void InputActionEvent(InputAction.CallbackContext context);

    public static event InputActionEvent leftButtonEvent;
    public static event InputActionEvent rightButtonEvent;

    public static Vector2 mousePos { get; private set; }

    InputActions inputActions;
    Camera mainCamera;


    private void OnEnable()
    {
        //initialize input actions
        inputActions = new InputActions();
        inputActions.Mouse.SetCallbacks(this);
        inputActions.Enable();

        mainCamera = Camera.main;
    }
    private void OnDisable() => inputActions.Disable();


    // calls the events

    public void OnLeftButton(InputAction.CallbackContext context) => leftButtonEvent(context);
    public void OnRightButton(InputAction.CallbackContext context) => rightButtonEvent(context);
    public void OnMousePos(InputAction.CallbackContext context)
    {
        var screenPos = context.ReadValue<Vector2>();
        mousePos = mainCamera.ScreenToWorldPoint(screenPos);
    }
}
