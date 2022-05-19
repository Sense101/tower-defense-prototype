using System;
using System.Runtime;
using TMPro;
using UnityEngine;

// this is the script that will handle all the interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    // set in inspector - the different parts of the interface
    [Header("Main Interface")]
    public GameObject mainInterface;

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
            mainInterface.transform.localPosition = new Vector2(-3.5f, value ? 2f : -10f);

            if (value)
            {
                // we've selected a turret, update the interface
                //@TODO make it hide and show
                turretName.text = _selectedTurret.gameObject.name;
                targetTypeSelector.SelectButton((int)value.targetType);

                // update the range preview
                rangePreviewObject.transform.position = _selectedTurret.transform.position;
                float rangeScale = 1 + (_selectedTurret.Info.Range * 2);
                rangePreviewObject.transform.localScale = new Vector2(rangeScale, rangeScale);
            }
            else
            {
                turretName.text = "";
                targetTypeSelector.DeselectAll();
            }
        }
    }

    // references
    TurretPlacer _turretPlacer;

    private void Start()
    {
        _turretPlacer = TurretPlacer.Instance;
    }

    private void Update()
    {

    }



    /////////// bunch of scripts called by the buttons within the interface
}
