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

    [Space(5)]
    public GameObject previewPane;
    public TextMeshProUGUI xpBar;
    public TextMeshProUGUI turretName;
    public UIButtonSelector targetTypeSelector;


    // internal variables
    private Turret _selectedTurret = null;

    // references
    TurretPlacer _turretPlacer;
    TurretPreviewController _turretPreviewController;
    AugmentationController _augController;

    private void Start()
    {
        _turretPlacer = TurretPlacer.Instance;
        _turretPreviewController = TurretPreviewController.Instance;
        _augController = AugmentationController.Instance;
    }

    private void Update()
    {

    }

    // bunch of scripts called by the buttons within the interface

    public void SellTurret()
    {
        Debug.Log("sell, todo");
        // then hide interface - setturret null
    }

    public void ApplyAugmentation(Augmentation aug, int index)
    {
        if (_selectedTurret)
        {
            AugmentationInfo info = _selectedTurret.Augmentations[index];
            info.augmentation = aug;
            info.currentTier = 1; //@TODO hard code tier 1 for now

            _augController.UpdateTurretAugmentations(_selectedTurret);
        }
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
            xpBar.text = turret.statistics.xp.ToString();

            // update the range preview
            float rangeScale = 1 + (turret.info.Range * 2);
            rangePreviewObject.transform.position = turret.transform.position;
            rangePreviewObject.transform.localScale = new Vector2(rangeScale, rangeScale);
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
        xpBar.text = _selectedTurret.statistics.xp.ToString();
    }
}
