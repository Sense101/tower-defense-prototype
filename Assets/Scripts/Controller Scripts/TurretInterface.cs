using System;
using System.Collections;
using System.Runtime;
using TMPro;
using UnityEngine;

// this is the script that will handle all the interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    // constants
    const float FADE_TIME = 1;

    // set in inspector - the different parts of the interface
    [Header("Main Interface")]
    public CanvasFadeGroup mainInterfaceGroup;

    [Space(5)]
    public GameObject previewPane;
    public GameObject xpBar;
    public TextMeshProUGUI turretName;
    public UIButtonSelector targetTypeSelector;


    // internal variables
    private Turret _selectedTurret = null;

    // references
    TurretPlacer _turretPlacer;
    TurretPreviewController _turretPreviewController;

    private void Start()
    {
        _turretPlacer = TurretPlacer.Instance;
        _turretPreviewController = TurretPreviewController.Instance;
    }

    private void Update()
    {

    }

    /////////// bunch of scripts called by the buttons within the interface


    // IMPORTANT: this cannot be called till after start
    // sets the turret
    public void SetTurret(Turret turret)
    {
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

            // update stuff within the interface - @TODO
            turretName.text = turret.gameObject.name;
            targetTypeSelector.SelectButton((int)turret.targetType);

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
}
