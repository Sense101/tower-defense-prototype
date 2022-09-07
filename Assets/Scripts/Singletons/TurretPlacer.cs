using UnityEngine;

public class TurretPlacer : Singleton<TurretPlacer>
{
    [SerializeField] Color unblockedColor;// = new HSVColor(255, 255, 255, 40);
    [SerializeField] Color blockedColor;// = new HSVColor(255, 0, 0, 10);
    [SerializeField] Color unblockedRangeColor;// = new HSVColor(150, 150, 150, 5);
    [SerializeField] Color blockedRangeColor;// = new HSVColor(200, 0, 0, 5);

    public enum State { canPlace, cannotPlace, choosingMutation }

    // set in inspector
    public SpriteRenderer turretPreview;
    public SpriteRenderer rangePreview;
    public SpriteRenderer mutationPreview;

    // internal variables
    private bool _overUI = false;
    private Turret _currentTurretPrefab = null;
    private Vector2Int _currentMouseTile = Vector2Int.zero;
    private State _state = State.cannotPlace;

    Map _map;
    TurretController _turretController;
    TurretInterface _turretInterface;
    MutationInterface _mutationInterface;
    EnemyController _enemyController;

    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        _turretInterface = TurretInterface.Instance;
        _mutationInterface = MutationInterface.Instance;
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

        if (_state == State.choosingMutation)
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
            if (_state == State.choosingMutation)
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

            if (_currentTurretPrefab && _state != State.choosingMutation)
            {
                RecalculateCanPlace();
            }
        }
    }

    private void OnOverUIEvent(bool overUI)
    {
        _overUI = overUI;

        if (_state != State.choosingMutation)
        {
            UpdatePreviews();
        }
    }

    private void SetChoosingUpgrade(Turret turret)
    {
        if (turret)
        {
            _mutationInterface.Open();
            _state = State.choosingMutation;
        }
        else
        {
            _mutationInterface.Open();
            RecalculateCanPlace();
        }
    }

    // recalculates if we can place at the current tile and updates the previews
    private void RecalculateCanPlace()
    {
        MoveToCurrentTile();

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


        // set the color
        Color newColor = _state == State.canPlace ? unblockedColor : blockedColor;
        turretPreview.color = newColor;
        rangePreview.color = _state == State.canPlace ? unblockedRangeColor : blockedRangeColor;

        UpdatePreviews();

    }

    public void MoveToCurrentTile()
    {
        transform.position = _map.MapToWorldSpace(_currentMouseTile);
    }

    // updates the previews
    private void UpdatePreviews()
    {
        Turret turretUnderneath = _map.GetTurret(_currentMouseTile);

        bool hideOverUi = _overUI && !(_state == State.choosingMutation);
        bool placerActive = _currentTurretPrefab && !hideOverUi;

        turretPreview.gameObject.SetActive(placerActive && !turretUnderneath);

        // range preview also shows when there is a turret selected
        rangePreview.gameObject.SetActive(_turretInterface.GetTurret() || placerActive);

        mutationPreview.gameObject.SetActive(placerActive && turretUnderneath);
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

    public void UpgradeTurret(TurretInfo info)
    {
        Turret turret = _map.GetTurretWorldSpace(transform.position);
        _turretController.ModifyTurret(turret, info);
    }
}
