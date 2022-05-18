using UnityEngine;
using UnityEngine.InputSystem;

public class TurretPlacer : Singleton<TurretPlacer>
{
    Color UNBLOCKED_COLOR = new Color(255, 255, 255, 0.4f);
    Color BLOCKED_COLOR = new Color(255, 0, 0, 0.1f);

    // set in inspector
    public SpriteRenderer turretPreview;
    public SpriteRenderer rangePreview;

    // stored variables
    private bool _overUI = false;
    private GameObject _currentTurretPrefab = null;
    public GameObject CurrentTurretPrefab
    {
        get
        {
            return _currentTurretPrefab;
        }
        set
        {
            _currentTurretPrefab = value;
            if (value)
            {
                RecalculateCanPlace();
            }

            UpdateActivePreviews();
        }
    }

    private Vector2Int _currentMouseTile = Vector2Int.zero;
    private bool _canPlace = false;

    Map _map;
    TurretController _turretController;
    EnemyController _enemyController;
    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        _enemyController = EnemyController.Instance;

        InputController.mouseMoveEvent += OnMouseMoveEvent;
        InputController.overUIEvent += OnOverUIEvent;
    }

    public bool TryPlaceTurret()
    {
        if (!CurrentTurretPrefab)
        {
            Debug.Log("No turret to place");
            return false;
        }

        if (!_canPlace)
        {
            Debug.Log("can't place turret");
            return false;
        }

        // create the turret
        var newTurret = Instantiate(
                CurrentTurretPrefab,
                (Vector3Int)_currentMouseTile,
                Quaternion.identity,
                transform
            ).GetComponent<Turret>();

        // initialize
        newTurret.Initialize();

        // add to the controller
        _turretController._turrets.Add(newTurret);
        _map.SetTurretWorldSpace(_currentMouseTile, newTurret);

        // deselect, temp
        //UIController.Instance.hotbar.DeselectAll();
        //CurrentTurretPrefab = null;

        RecalculateCanPlace();

        return true;
    }

    private void OnMouseMoveEvent(Vector2 mousePos)
    {
        Vector2Int newMouseTile = Vector2Int.RoundToInt(mousePos);
        if (newMouseTile != _currentMouseTile)
        {
            _currentMouseTile = newMouseTile;
            if (CurrentTurretPrefab)
            {
                RecalculateCanPlace();
            }
        }
    }

    private void OnOverUIEvent(bool overUI)
    {
        _overUI = overUI;
        UpdateActivePreviews();
    }

    // recalculates if we can place at the current tile and updates the preview
    private void RecalculateCanPlace()
    {
        Debug.Log("ok");
        // we are at a new tile, recalculate if we can place
        _canPlace = _map.CanPlaceAtTileWorldSpace(_currentMouseTile);

        // move the previews
        turretPreview.transform.position = (Vector3Int)_currentMouseTile;
        rangePreview.transform.position = (Vector3Int)_currentMouseTile;

        // set the color
        Color newColor = _canPlace ? UNBLOCKED_COLOR : BLOCKED_COLOR;
        turretPreview.color = newColor;

        UpdateActivePreviews();
    }

    // updates the previews
    private void UpdateActivePreviews()
    {
        turretPreview.gameObject.SetActive(_currentTurretPrefab && !_overUI);
        rangePreview.gameObject.SetActive(_currentTurretPrefab && !_overUI && _canPlace);
    }
}
