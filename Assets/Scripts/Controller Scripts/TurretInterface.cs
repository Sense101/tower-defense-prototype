using System;
using System.Collections;
using System.Runtime;
using TMPro;
using UnityEngine;

// this is the script that will handle all the interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    // set in inspector - the different parts of the interface
    [SerializeField] TextMeshProUGUI xpBar;
    [SerializeField] TextMeshProUGUI turretName;
    [SerializeField] UIButtonSelector targetTypeSelector;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI rangeText;


    // internal variables
    private Turret _selectedTurret = null;
    private CanvasFadeGroup _fadeGroup;

    // references
    TurretPlacer _turretPlacer;
    TurretController _turretController;
    TurretPreview _turretPreviewController; //@TODO merge preview controller into interface script?
    AugmentController _augmentController;
    AugmentInterface _augmentInterface;
    Map _map;

    private void Start()
    {
        // set references
        _turretPlacer = TurretPlacer.Instance;
        _turretController = TurretController.Instance;
        _turretPreviewController = TurretPreview.Instance;
        _augmentController = AugmentController.Instance;
        _augmentInterface = AugmentInterface.Instance;
        _map = Map.Instance;

        _fadeGroup = GetComponent<CanvasFadeGroup>();
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

    // called by buttons
    public void ApplyAugment(int index)
    {

        if (!_selectedTurret)
        {
            // no turret selected, do nothing
            return;
        }
        Augment augment;
        int tier;
        if (index == 0)
        {
            augment = _augmentController.damageAugment;
            _selectedTurret.damageAugmentLevel++;
            tier = _selectedTurret.damageAugmentLevel;
            damageText.text = $"damage upgrade ({tier}/5)";
        }
        else
        {
            augment = _augmentController.rangeAugment;
            tier = _selectedTurret.rangeAugmentLevel++;
            rangeText.text = $"range upgrade ({tier}/5)";
        }

        _augmentController.ApplyAugment(_selectedTurret.stats, augment.type, tier);
        //@TODO
        _turretPlacer.UpdateRangeScale(_selectedTurret);
    }
    public void ChooseAugment()
    {
        _augmentInterface.Show();
    }

    // IMPORTANT: this cannot be called till after start
    // selects the turret
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
            _selectedTurret.onBulletHitEvent.RemoveListener(OnTurretHit);
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
            _fadeGroup.Show();

            // add a hit event listener so we can update as the new turret changes
            turret.onBulletHitEvent.AddListener(OnTurretHit);

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
            _fadeGroup.Hide();
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
