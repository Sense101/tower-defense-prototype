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

    private Turret _selectedTurret = null;
    public Turret SelectedTurret
    {
        get
        {
            return _selectedTurret;
        }
        // IMPORTANT: this should never be set before start
        set
        {
            _selectedTurret = value;

            // set visibility
            GameObject rangePreviewObject = _turretPlacer.rangePreview.gameObject;
            rangePreviewObject.SetActive(value);

            if (value)
            {
                // we've selected a turret, update the interface
                turretName.text = _selectedTurret.gameObject.name;
                targetTypeSelector.SelectButton((int)value.targetType);
                mainInterfaceGroup.Show();

                // update the range preview
                rangePreviewObject.transform.position = _selectedTurret.transform.position;
                float rangeScale = 1 + (_selectedTurret.info.Range * 2);
                rangePreviewObject.transform.localScale = new Vector2(rangeScale, rangeScale);

                // update the actual preview
                _turretPreviewController.SetPreview(value);
            }
            else
            {
                turretName.text = "";
                targetTypeSelector.DeselectAll();
                mainInterfaceGroup.Hide();
            }
        }
    }

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
}
