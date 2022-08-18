using UnityEngine;
using UnityEngine.UI;

public class UpgradeInterface : Singleton<UpgradeInterface>
{
    // set in inspector
    [SerializeField] Image path1;
    [SerializeField] Image path2;
    [SerializeField] Image path3;

    // internal variables
    private Turret _selectedTurret;

    // references
    CanvasFadeGroup _fadeGroup;
    TurretController _turretController;
    TurretPlacer _turretPlacer;
    private void Start()
    {
        // set references
        _fadeGroup = GetComponent<CanvasFadeGroup>();
        _turretController = TurretController.Instance;
        _turretPlacer = TurretPlacer.Instance;
    }

    public void Show(Turret turret)
    {
        _selectedTurret = turret;

        // update previews - still work @todo here
        path1.sprite = _selectedTurret.info.path1.fullSprite;
        path2.sprite = _selectedTurret.info.path2.fullSprite;
        path3.sprite = _selectedTurret.info.path3.fullSprite;

        _fadeGroup.Show();
    }
    public void Hide()
    {
        _selectedTurret = null;

        _fadeGroup.Hide();
    }

    // called by upgrade path buttons
    public void SelectPath1()
    {
        SelectPathInternal(_selectedTurret.info.path1);
    }
    public void SelectPath2()
    {
        SelectPathInternal(_selectedTurret.info.path2);
    }
    public void SelectPath3()
    {
        SelectPathInternal(_selectedTurret.info.path3);
    }

    private void SelectPathInternal(TurretInfo newInfo)
    {
        // upgrade the turret
        _turretController.ModifyTurret(_selectedTurret, newInfo);

        // deselect, our job is done
        _turretPlacer.TryDeselectTurret();
    }
}
