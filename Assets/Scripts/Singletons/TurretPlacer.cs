using UnityEngine;

public class TurretPlacer : Singleton<TurretPlacer>
{
    [SerializeField] Color unblockedColor;
    [SerializeField] Color blockedColor;
    [SerializeField] Color unblockedRangeColor;
    [SerializeField] Color blockedRangeColor;

    public bool _canPlace = false;

    // set in inspector
    public SpriteRenderer turretPreview;
    public SpriteRenderer rangePreview;

    // internal variables
    private bool _overUI = false;
    private Turret _currentTurretPrefab = null;
    private Vector2Int _currentMouseTile = Vector2Int.zero;

    Map _map;
    TurretController _turretController;
    TurretInterface _turretInterface;
    EnemyController _enemyController;

    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        _turretInterface = TurretInterface.Instance;
        _enemyController = EnemyController.Instance;

        InputController.mouseMoveEvent.AddListener(OnMouseMoveEvent);
        InputController.overUIEvent.AddListener(OnOverUIEvent);
    }

    public void SetTurretPrefab(Turret turretPrefab)
    {
        _currentTurretPrefab = turretPrefab;
        if (turretPrefab)
        {
            _turretInterface.SetTurret(null);
            UpdateRangeScale(turretPrefab, true);
            RecalculateCanPlace();
        }
        else
        {
            // make sure range color is back to normal
            rangePreview.color = unblockedRangeColor;
        }

        UpdatePreviews();
    }

    public Turret GetTurretPrefab()
    {
        return _currentTurretPrefab;
    }

    public bool TryPlaceTurret()
    {
        if (!_currentTurretPrefab)
        {
            Debug.Log("No turret to place");
            return false;
        }

        if (!_canPlace)
        {
            // can't place a turret at the current location
            return false;
        }

        // create the turret
        Turret newTurret = _turretController.PlaceTurret(_map.MapToWorldSpace(_currentMouseTile));

        // add it to the map
        _map.SetTurret(_currentMouseTile, newTurret);

        // deselect, temp @todo this is really badly done
        MainInterface.Instance.hotbar.DeselectAll();
        SetTurretPrefab(null);

        // connect interface to new turret
        _turretInterface.SetTurret(newTurret);

        if (_currentTurretPrefab)
        {
            RecalculateCanPlace();
        }

        return true;
    }

    public bool TryDeselectTurret()
    {
        // try and deselect the turret
        if (_currentTurretPrefab)
        {
            // deselect the turret
            SetTurretPrefab(null);

            return true;
        }

        // none selected
        return false;
    }

    private void OnMouseMoveEvent(Vector2 mousePos)
    {
        Vector2Int newMouseTile = _map.WorldToMapSpace(mousePos);

        if (newMouseTile != _currentMouseTile)
        {
            _currentMouseTile = newMouseTile;

            if (_currentTurretPrefab)
            {
                RecalculateCanPlace();
            }
        }
    }

    private void OnOverUIEvent(bool overUI)
    {
        _overUI = overUI;
        UpdatePreviews();
    }

    // recalculates if we can place at the current tile and updates the previews
    private void RecalculateCanPlace()
    {
        MoveToCurrentTile();

        // we are at a new tile, recalculate if we can place
        _canPlace = _map.IsTilePlaceable(_currentMouseTile);

        // set the color
        Color newColor = _canPlace ? unblockedColor : blockedColor;
        turretPreview.color = newColor;
        rangePreview.color = _canPlace ? unblockedRangeColor : blockedRangeColor;

        UpdatePreviews();
    }

    public void MoveToCurrentTile()
    {
        transform.position = _map.MapToWorldSpace(_currentMouseTile);
    }

    // updates the previews
    private void UpdatePreviews()
    {
        bool placerActive = _currentTurretPrefab && !_overUI;

        turretPreview.gameObject.SetActive(placerActive);

        // range preview also shows when there is a turret selected
        rangePreview.gameObject.SetActive(_turretInterface.GetTurret() || placerActive);
    }

    public void UpdateRangeScale(Turret turret, bool useInfo = false)
    {
        float rangeScale;
        if (useInfo)
        {
            rangeScale = turret.info.rangeModifier * 2;
        }
        else
        {
            rangeScale = turret.stats.range * 2;
        }

        rangePreview.transform.localScale = new Vector2(rangeScale, rangeScale);
    }
}
