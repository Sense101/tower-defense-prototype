using UnityEngine;
using UnityEngine.InputSystem;

public class TurretPlacer : Singleton<TurretPlacer>
{
    Color UNBLOCKED_COLOR = new Color(255, 255, 255, 0.4f);
    Color BLOCKED_COLOR = new Color(255, 0, 0, 0.05f);

    // set in inspector
    [SerializeField] SpriteRenderer TurretPreview;

    // stored variables
    private GameObject _currentTurretPrefab = null;
    public GameObject CurrentTurretPrefab
    {
        get
        {
            return _currentTurretPrefab;
        }
        set
        {
            // also set preview visibility here
            TurretPreview.gameObject.SetActive(value);
            _currentTurretPrefab = value;
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

        InputManager.leftButtonEvent += OnLeftButton;
        InputManager.rightButtonEvent += OnRightButton;
        InputManager.mouseMoveEvent += OnMouseMove;
    }

    private void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!CurrentTurretPrefab)
            {
                Debug.Log("No turret to place");
                return;
            }

            if (_canPlace)
            {
                Debug.Log("can place");

                // create the turret
                var newTurret = Instantiate(
                        CurrentTurretPrefab,
                        (Vector3Int)_currentMouseTile,
                        Quaternion.identity
                    ).GetComponent<Turret>();

                // initialize
                newTurret.Initialize();
                // add to the controller
                _turretController._turrets.Add(newTurret);
                _map.SetTurretWorldSpace(_currentMouseTile, newTurret);

                RecalculateCanPlace();
            }
            else
            {
                Debug.Log("cannot place");
            }
        }
    }
    private void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // @TODO simplify to one function
            var mousePos = Vector2Int.RoundToInt(InputManager.mousePos);
            var tileInfo = _map.TryGetTileWorldSpace(mousePos);

            // check to see if tile exists
            if (tileInfo == null) return;

            if (tileInfo.Turret)
            {
                Debug.Log("can destroy");
                Destroy(tileInfo.Turret.gameObject);
                tileInfo.Turret = null;
            }
            else
            {
                Debug.Log("cannot destroy");
            }
        }
    }

    private void OnMouseMove(Vector2 mousePos)
    {
        Vector2Int newMouseTile = Vector2Int.RoundToInt(mousePos);
        if (newMouseTile != _currentMouseTile)
        {
            _currentMouseTile = newMouseTile;
            RecalculateCanPlace();
        }
    }

    // recalculates if we can place at the current tile and updates the preview
    private void RecalculateCanPlace()
    {
        // we are at a new tile, recalculate if we can place
        _canPlace = _map.CanPlaceAtTileWorldSpace(_currentMouseTile);
        TurretPreview.transform.position = (Vector3Int)_currentMouseTile;

        // set the color
        Color newColor = _canPlace ? UNBLOCKED_COLOR : BLOCKED_COLOR;
        TurretPreview.color = newColor;
    }
}
