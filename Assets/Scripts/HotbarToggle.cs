using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarToggle : GroupToggle
{
    // set in inspector
    [SerializeField] Turret _turretPrefab = null;
    [SerializeField] Sprite _turretPreviewSprite = null;

    protected override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretPlacer.Instance.SetTurretPrefab(_turretPrefab);
            TurretPlacer.Instance.turretPreview.sprite = _turretPreviewSprite;
        }
        else
        {
            TurretPlacer.Instance.TryDeselectTurret();
        }

        // notify parent
        base.OnValueChanged(isOn);
    }

}
