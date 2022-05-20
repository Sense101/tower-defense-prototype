using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>, InputActions.IMouseActions
{
    // allow other scripts to fire when the mouse position changes
    public delegate void PositionEvent(Vector2 pos);
    public delegate void OverUIEvent(bool overUI);
    public static event PositionEvent mouseMoveEvent;
    public static event OverUIEvent overUIEvent;
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
        if (!context.started || !_active)
        {
            return;
        }

        // ignore input if over UI
        if (mouseOverUI)
        {
            return;
        }

        // then place turret if we have one selected
        if (_turretPlacer.CurrentTurretPrefab)
        {
            _turretPlacer.TryPlaceTurret();
            return;
        }

        // try and select a turret
        TileInfo currentTile = _map.TryGetTileWorldSpace(Vector2Int.RoundToInt(mousePos));
        _turretInterface.SelectedTurret = currentTile?.Turret;
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (!context.started || !_active)
        {
            return;
        }

        // stop placing
        if (_turretPlacer.CurrentTurretPrefab)
        {
            // deselect the turret
            UIController.Instance.hotbar.DeselectAll();
            _turretPlacer.CurrentTurretPrefab = null;
            return;
        }

        // deselect if a turret is selected
        if (_turretInterface.SelectedTurret)
        {
            _turretInterface.SelectedTurret = null;
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
            overUIEvent(newMouseOverUI);
            mouseOverUI = newMouseOverUI;
        }
        mouseMoveEvent(worldPos);
    }
}
