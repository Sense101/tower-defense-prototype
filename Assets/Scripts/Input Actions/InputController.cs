using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>, InputActions.IMouseActions
{
    // allow other scripts to fire when the mouse position changes
    public delegate void PositionEvent(Vector2 pos);
    public delegate void OverUIEvent(bool overUI);
    public static event PositionEvent mouseMoveEvent;
    public static event OverUIEvent overUIEvent;
    public static Vector2 MousePos { get; private set; }
    public static bool MouseOverUI { get; private set; }

    // prevents any inputs until start
    private bool _active = false;

    // references
    InputActions _inputActions;
    Camera _mainCamera;
    UIController _uiController;
    TurretPlacer _turretPlacer;


    // input actions stuff, wish this happened automatically
    private void OnEnable()
    {
        //initialize input actions
        _inputActions = new InputActions();
        _inputActions.Mouse.SetCallbacks(this);
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    private void Start()
    {
        // set references
        _mainCamera = Camera.main;
        _uiController = UIController.Instance;
        _turretPlacer = TurretPlacer.Instance;
        _active = true;
    }


    // handles the hierarchy for what gets priority when an input is detected

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (!context.started || !_active)
        {
            return;
        }

        // ignore input if over UI
        if (MouseOverUI)
        {
            return;
        }

        // then place turret if we have one selected
        if (_turretPlacer.TryPlaceTurret())
        {
            // nice, we placed a turret
        }

        // otherwise select a turret?
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (!context.started || !_active)
        {
            return;
        }

        if (_turretPlacer.CurrentTurretPrefab)
        {
            // deselect the turret
            UIController.Instance.hotbar.DeselectAll();
            _turretPlacer.CurrentTurretPrefab = null;
        }
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        if (!_active)
        {
            return;
        }

        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);

        MousePos = worldPos;
        bool newMouseOverUI = _uiController.IsWithinPrimaryElement(worldPos);
        if (newMouseOverUI != MouseOverUI)
        {
            // it's changed, send an event
            Debug.Log("change");
            overUIEvent(newMouseOverUI);
            MouseOverUI = newMouseOverUI;
        }
        mouseMoveEvent(worldPos);
    }
}
