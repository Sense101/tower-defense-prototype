using System;
using System.Collections;
using System.Runtime;
using TMPro;
using UnityEngine;

// this is the script that will handle all the interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    // set in inspector - the different parts of the interface
    [Header("Main Interface")]
    public CanvasFadeGroup mainInterfaceGroup;
    public GameObject previewPane;
    public TextMeshProUGUI xpBar;
    public TextMeshProUGUI turretName;
    public UIButtonSelector targetTypeSelector;

    [Header("Upgrade Interface")]
    public CanvasFadeGroup upgradeInterfaceGroup;
    public TurretInfo doubleInfo; //temp


    // internal variables
    private Turret _selectedTurret = null;

    // references
    TurretPlacer _turretPlacer;
    TurretController _turretController;
    TurretPreviewController _turretPreviewController; //@TODO merge preview controller into interface script?
    AugmentController _augController;
    Map _map;

    private void Start()
    {
        // set references
        _turretPlacer = TurretPlacer.Instance;
        _turretController = TurretController.Instance;
        _turretPreviewController = TurretPreviewController.Instance;
        _augController = AugmentController.Instance;
        _map = Map.Instance;
    }

    // bunch of scripts called by the buttons within the interface

    public void SellTurret()
    {
        if (!_selectedTurret)
        {
            // we don't have a turret selected
            return;
        }

        Debug.Log("selling turret");
        // sell the turret
        _turretController.SellTurret(_selectedTurret);

        // set the tile to not be holding a turret
        _map.SetTurretWorldSpace(Vector2Int.RoundToInt(_selectedTurret.transform.position), null);

        // hide interface
        SetTurret(null);
    }

    public void ApplyAugmentation()
    {
        if (_selectedTurret)
        {
            AugmentationInfo info = _selectedTurret.customAugment;
            info.augmentation = _augController.ChooseNewAugmentation(info.augmentation);
            info.currentTier = 1; //@TODO hard code tier 1 for now

            //_augController.UpdateTurretAugmentations(_selectedTurret);
        }
    }

    public void UpgradeTurret()
    {
        Debug.Log("did");
        _turretPlacer.UpgradeTurret(doubleInfo);
        _turretPlacer.TryDeselectTurret();
    }


    // IMPORTANT: this cannot be called till after start
    // sets the turret
    public void SetTurret(Turret turret)
    {
        if (_selectedTurret == turret)
        {
            // it's the same
            return;
        }

        if (_selectedTurret)
        {
            // we are changing, remove the event link from the old turret
            _selectedTurret.onHitEvent.RemoveListener(OnTurretHit);
        }

        // actually update the reference
        _selectedTurret = turret;

        // set the range preview visibility
        GameObject rangePreviewObject = _turretPlacer.rangePreview.gameObject;
        rangePreviewObject.SetActive(turret);

        // update the turret preview
        _turretPreviewController.SetPreview(turret);

        if (turret)
        {
            // we've selected a turret, show the main interface
            mainInterfaceGroup.Show();

            // add a hit event listener so we can update as the new turret changes
            turret.onHitEvent.AddListener(OnTurretHit);

            // update stuff within the interface - @TODO
            turretName.text = turret.gameObject.name;
            targetTypeSelector.SelectButton((int)turret.targetType);
            xpBar.text = turret.stats.xp.ToString();

            // update the range preview
            _turretPlacer.UpdateRangeScale(turret);
            rangePreviewObject.transform.position = turret.transform.position;
        }
        else
        {
            mainInterfaceGroup.Hide();
        }
    }

    public Turret GetTurret()
    {
        return _selectedTurret;
    }

    // called when the current turret hits an enemy
    private void OnTurretHit()
    {
        // update xp
        xpBar.text = _selectedTurret.stats.xp.ToString();
    }
}
