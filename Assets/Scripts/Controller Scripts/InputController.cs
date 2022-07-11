using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>, InputActions.IMouseActions
{
    // allow other scripts to fire when the mouse position changes
    public static UnityEvent<Vector2> mouseMoveEvent = new UnityEvent<Vector2>();
    public static UnityEvent<bool> overUIEvent = new UnityEvent<bool>();
    public static Vector2 mousePos { get; private set; }
    public static bool mouseOverUI { get; private set; }

    // prevents any inputs until start
    private bool _active = false;

    // references
    InputActions _inputActions;
    Camera _mainCamera;
    UIController _uiController;
    TurretPlacer _turretPlacer;
    TurretInterface _turretInterface;
    Map _map;


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
        _turretInterface = TurretInterface.Instance;
        _map = Map.Instance;
        _active = true;
    }


    // handles the hierarchy for what gets priority when an input is detected

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (!context.performed || !_active)
        {
            return;
        }

        // ignore input if over UI
        if (mouseOverUI)
        {
            return;
        }

        //@TODO check if we have any overlays open, and if so close them first

        // then place turret if we have one selected
        if (_turretPlacer.GetTurretPrefab())
        {
            _turretPlacer.TryPlaceTurret();
            return;
        }

        // try and select a turret
        TileInfo currentTile = _map.TryGetTileWorldSpace(Vector2Int.RoundToInt(mousePos));
        _turretInterface.SetTurret(currentTile?.Turret);
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (!context.performed || !_active)
        {
            return;
        }

        // stop placing
        if (_turretPlacer.TryDeselectTurret())
        {
            return;
        }

        // deselect if a turret is selected
        if (_turretInterface.GetTurret())
        {
            _turretInterface.SetTurret(null);
            return;
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

        mousePos = worldPos;
        bool newMouseOverUI = _uiController.IsWithinPrimaryElement(worldPos);
        if (newMouseOverUI != mouseOverUI)
        {
            // it's changed, send an event
            overUIEvent.Invoke(newMouseOverUI);
            mouseOverUI = newMouseOverUI;
        }
        mouseMoveEvent.Invoke(worldPos);
    }
}
