using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class InputController : Singleton<InputController>, InputActions.IMouseActions
{
    // allow other scripts to fire when the mouse position changes
    public static UnityEvent<Vector2> mouseMoveEvent = new UnityEvent<Vector2>();
    public static UnityEvent<bool> overUIEvent = new UnityEvent<bool>();
    public static Vector2 mousePos { get; private set; }
    public static bool mouseOverUI { get; private set; }

    // the input actions - for every script to use to check inputs
    public InputActions input;

    // input overlays - prevent inputs till closed
    [SerializeField] private List<UIOverlay> _inputOverlays = new List<UIOverlay>(); // @todo serialized for debug purposes

    // prevents any inputs until start
    private bool _active = false;

    // references
    Camera _mainCamera;
    TurretPlacer _turretPlacer;
    TurretInterface _turretInterface;
    Map _map;


    // input actions stuff, wish this happened automatically
    private void OnEnable()
    {
        //initialize input actions
        input = new InputActions();
        input.Mouse.SetCallbacks(this);
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void Start()
    {
        // set references
        _mainCamera = Camera.main;
        _turretPlacer = TurretPlacer.Instance;
        _turretInterface = TurretInterface.Instance;
        _map = Map.Instance;
        _active = true;
    }
    public void AddInputOverlay(UIOverlay overlay)
    {
        _inputOverlays.Add(overlay);
        MainInterface.Instance.overlayFilter.Show();
    }
    public void RemoveInputOverlay(UIOverlay overlay)
    {
        _inputOverlays.Remove(overlay);
        if (_inputOverlays.Count <= 0)
        {
            MainInterface.Instance.overlayFilter.Hide();
        }
    }

    private void Update()
    {
        if (!_active || Mouse.current == null)
        {
            return;
        }

        // check for changes to over UI state
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
        mousePos = worldPos;

        bool newMouseOverUI = IsMouseOverUI();
        if (newMouseOverUI != mouseOverUI)
        {
            // it's changed, send an event
            overUIEvent.Invoke(newMouseOverUI);
            mouseOverUI = newMouseOverUI;
        }
        mouseMoveEvent.Invoke(worldPos);
    }


    // handles the hierarchy for what gets priority when an input is detected

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (!context.canceled || !_active)
        {
            return;
        }

        // ignore input if over UI
        if (mouseOverUI)
        {
            return;
        }

        // if there is an overlay open, close it and ignore this input
        if (_inputOverlays.Count > 0)
        {
            _inputOverlays[0].Close();
            return;
        }

        // then place turret if we have one selected
        if (_turretPlacer.GetTurret())
        {
            _turretPlacer.TryPlaceTurret();
            return;
        }

        // try and select a turret
        TileInfo currentTile = _map.TryGetTileWorldSpace(mousePos);
        if (currentTile != null)
        {
            _turretInterface.SetTurret(currentTile.Turret);
        }
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (!context.canceled || !_active)
        {
            return;
        }

        // if there is an overlay open, close it and ignore this input
        if (_inputOverlays.Count > 0)
        {
            _inputOverlays[0].Close();
            return;
        }

        // if there is a prefab selected to place, stop placing
        if (_turretPlacer.GetTurret())
        {
            _turretPlacer.DeselectTurret();
            MainInterface.Instance.hotbarSelector.DeselectAll();
            return;
        }

        // deselect if a turret is selected
        if (_turretInterface.GetTurret())
        {
            _turretInterface.SetTurret(null);
            return;
        }
    }

    private bool IsMouseOverUI()
    {
        if (EventSystem.current == null || Mouse.current == null)
        {
            return false;
        }

        InputSystemUIInputModule inputModule = ((InputSystemUIInputModule)EventSystem.current.currentInputModule);
        RaycastResult lastRaycastResult = inputModule.GetLastRaycastResult(Mouse.current.deviceId);

        const int uiLayer = 5;
        return lastRaycastResult.gameObject != null && lastRaycastResult.gameObject.layer == uiLayer;
    }
}
