using UnityEngine;

public class TurretPlacer : Singleton<TurretPlacer>
{
    public bool canPlace = false;

    // set in inspector
    public SpriteRenderer turretPreview;
    public SpriteRenderer rangePreview;

    // internal variables
    private bool _overUI = false;
    private Turret _currentTurret = null;
    private Vector2Int _currentMouseTile = Vector2Int.zero;

    Map _map;
    InputController _inputController;
    CoinController _coinController;
    TurretInterface _turretInterface;
    ConfigController _configs;

    private void Start()
    {
        _map = Map.Instance;
        _inputController = InputController.Instance;
        _coinController = CoinController.Instance;
        _turretInterface = TurretInterface.Instance;
        _configs = ConfigController.Instance;

        InputController.mouseMoveEvent.AddListener(OnMouseMoveEvent);
        InputController.overUIEvent.AddListener(OnOverUIEvent);
    }

    public void SetTurret(Turret turret)
    {
        _currentTurret = turret;
        if (turret)
        {
            _turretInterface.SetTurret(null);
            UpdateRangeScale(turret, true);
            RecalculateCanPlace();
        }
        else
        {
            // make sure range color is back to normal
            rangePreview.color = _configs.color.unblockedRangeColor;
        }

        UpdatePreviews();
    }

    public Turret GetTurret()
    {
        return _currentTurret;
    }

    public bool TryPlaceTurret()
    {
        if (!_currentTurret)
        {
            Debug.LogError("Tried to place turret with none selected!");
            return false;
        }

        if (!canPlace)
        {
            // can't place a turret at the current location
            return false;
        }

        // NOTE: if we succesfully place but then cannot afford another, the hotbar item will deselect for us
        if (!_configs.debug.noCoinCost && !_coinController.TrySpendCoins(_currentTurret.info.cost))
        {
            Debug.LogError("Can not afford turret to place - It should not have been selected!");
            return false;
        }

        // create the turret
        Turret newTurret = TurretController.Instance.PlaceTurret(_map.MapToWorldSpace(_currentMouseTile));

        // add it to the map
        _map.SetTurret(_currentMouseTile, newTurret);

        // connect interface to new turret
        _turretInterface.SetTurret(newTurret);

        if (!_inputController.input.Modifiers.Multiplace.IsPressed())
        {
            // deselect, we are not multiplacing
            MainInterface.Instance.hotbarSelector.DeselectAll();
            SetTurret(null);
        }

        if (_currentTurret)
        {
            // if we still have a turret selected recalculate
            RecalculateCanPlace();
        }

        return true;
    }

    public void DeselectTurret()
    {
        if (_currentTurret)
        {
            // deselect the turret
            SetTurret(null);
        }
    }

    private void OnMouseMoveEvent(Vector2 mousePos)
    {
        Vector2Int newMouseTile = _map.WorldToMapSpace(mousePos);

        if (newMouseTile != _currentMouseTile)
        {
            _currentMouseTile = newMouseTile;

            if (_currentTurret)
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
        canPlace = _map.IsTilePlaceable(_currentMouseTile);

        // set the color
        Color newColor = canPlace ? _configs.color.unblockedPlacementColor : _configs.color.blockedPlacementColor;
        turretPreview.color = newColor;
        rangePreview.color = canPlace ? _configs.color.unblockedRangeColor : _configs.color.blockedRangeColor;

        UpdatePreviews();
    }

    public void MoveToCurrentTile()
    {
        transform.position = _map.MapToWorldSpace(_currentMouseTile);
    }

    // updates the previews
    private void UpdatePreviews()
    {
        bool placerActive = _currentTurret && !_overUI;

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
