using UnityEngine;

public class TurretPlacer : Singleton<TurretPlacer>
{
    //@TODO change all these to hsvcolor
    readonly Color UNBLOCKED_COLOR = new Color(255, 255, 255, 0.4f);
    readonly Color BLOCKED_COLOR = new Color(255, 0, 0, 0.1f);
    readonly Color RANGE_UNBLOCKED_COLOR = new Color(150, 150, 150, 0.05f);
    readonly Color RANGE_BLOCKED_COLOR = new Color(200, 0, 0, 0.05f);

    public enum State { canPlace, cannotPlace, choosingUpgrade }

    // set in inspector
    public SpriteRenderer turretPreview;
    public SpriteRenderer rangePreview;

    // internal variables
    private bool _overUI = false;
    private Turret _currentTurretPrefab = null;
    private Vector2Int _currentMouseTile = Vector2Int.zero;
    private State _state = State.cannotPlace;

    Map _map;
    TurretController _turretController;
    TurretInterface _turretInterface;
    UpgradeInterface _upgradeInterface;
    EnemyController _enemyController;

    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        _turretInterface = TurretInterface.Instance;
        _upgradeInterface = UpgradeInterface.Instance;
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
            rangePreview.color = RANGE_UNBLOCKED_COLOR;
        }

        UpdateActivePreviews();
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

        if (_state == State.choosingUpgrade)
        {
            // stop choosing an upgrade
            SetChoosingUpgrade(null);
            UpdateRangeScale(_currentTurretPrefab, true);
            return false;
        }

        if (_state == State.cannotPlace)
        {
            // can't place a turret at the current location
            return false;
        }

        Turret turret = _map.GetTurret(_currentMouseTile);
        if (turret)
        {
            // trying to place a turret where one exists, time for some merging!
            SetChoosingUpgrade(turret);
            UpdateRangeScale(turret);
            return false;
        }

        // create the turret
        Turret newTurret = _turretController.PlaceTurret(_currentMouseTile * 10);

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
            if (_state == State.choosingUpgrade)
            {
                // reset back to normal, we are cancelling
                SetChoosingUpgrade(null);
            }

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
        // don't update anything if we're choosing an upgrade
        if (newMouseTile != _currentMouseTile)
        {
            _currentMouseTile = newMouseTile;
            if (_currentTurretPrefab && _state != State.choosingUpgrade)
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

    private void SetChoosingUpgrade(Turret turret)
    {
        if (turret)
        {
            _upgradeInterface.Show(turret);
            _state = State.choosingUpgrade;
        }
        else
        {
            _upgradeInterface.Hide();
            RecalculateCanPlace();
        }
    }

    // recalculates if we can place at the current tile and updates the previews
    private void RecalculateCanPlace()
    {
        // we are at a new tile, recalculate if we can place
        bool isPlaceable = _map.IsTilePlaceable(_currentMouseTile);
        if (isPlaceable)
        {
            _state = State.canPlace;
        }
        else
        {
            _state = State.cannotPlace;
        }

        // move the previews
        UpdatePreviewPosition(_currentMouseTile * 10);

        // set the color
        Color newColor = _state == State.canPlace ? UNBLOCKED_COLOR : BLOCKED_COLOR;
        turretPreview.color = newColor;
        rangePreview.color = _state == State.canPlace ? RANGE_UNBLOCKED_COLOR : RANGE_BLOCKED_COLOR;

        UpdateActivePreviews();
    }

    // updates the previews
    private void UpdateActivePreviews()
    {
        bool hideOverUi = _overUI && !(_state == State.choosingUpgrade);
        bool placerActive = _currentTurretPrefab && !hideOverUi;

        turretPreview.gameObject.SetActive(placerActive);

        // range preview also shows when there is a turret selected
        rangePreview.gameObject.SetActive(_turretInterface.GetTurret() || placerActive);
    }

    private void UpdatePreviewPosition(Vector2 position)
    {
        turretPreview.transform.position = position;
        rangePreview.transform.position = position;
    }

    public void UpdateRangeScale(Turret turret, bool useInfo = false)
    {
        float rangeScale;
        if (useInfo)
        {
            rangeScale = turret.info.RangeModifier * 2;
        }
        else
        {
            rangeScale = turret.stats.range * 2;
        }

        rangePreview.transform.localScale = new Vector2(rangeScale, rangeScale);
    }

    public void UpgradeTurret(TurretInfo info)
    {
        Turret turret = _map.GetTurretWorldSpace(Vector2Int.RoundToInt(rangePreview.transform.position));
        _turretController.ModifyTurret(turret, info);
    }
}
